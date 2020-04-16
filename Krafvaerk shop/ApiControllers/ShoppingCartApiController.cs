using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Krafvaerk_shop
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartApiController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IConfiguration _configuration;
        private static string _cartKey;
        public string CartKey
        {
            get
            {
                if (_cartKey == null)
                {
                    _cartKey = _configuration.GetValue<string>("CartKey");
                }
                return _cartKey;
            }
        }

        public ShoppingCartApiController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration)
        {
            _repositoryWrapper = repositoryWrapper;
            _configuration = configuration;
        }

        // GET: api/<controller>
        [HttpGet("GetShoppingCart")]
        public IActionResult GetShoppingCart()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShoppingCart cart = GetOrCreateShoppingCart();
            if (cart != null)
            {
                // send only necessary values
                var apiCart = new ShoppingCartDTO(cart.Id, cart.CartDataJSON);
                return new JsonResult(apiCart);
            }
            return BadRequest();
        }

        private ShoppingCart GetOrCreateShoppingCart()
        {
            string cartId = HttpContext.Session.GetString(CartKey);
            if (!string.IsNullOrEmpty(CartKey))
            {
                try
                {
                    ShoppingCart cart = _repositoryWrapper.IShoppingCartRepository.GetByCondition(x => x.Id == cartId);
                    if (cart == null)
                    {
                        cart = CreateShoppingCart(cartId);
                    }
                    else if (cart.Expires < DateTime.Today)
                    {
                        //remove expired cart and create new
                        _repositoryWrapper.IShoppingCartRepository.Delete(cart);
                        _repositoryWrapper.Save();
                        cart = CreateShoppingCart(cartId);
                    }
                    return cart;
                }
                catch (Exception e)
                {
                    _repositoryWrapper.ILog.Create(new Log { Logtype = LOG_TYPE.Error, Message = e.Message, Timestamp = DateTime.Now });
                    _repositoryWrapper.Save();
                }
            }
            return null;
        }

        /// <summary>
        /// Creates cart to db and adds a temporary order value as json
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        private ShoppingCart CreateShoppingCart(string cartId)
        {
            ShoppingCart cart = new ShoppingCart { Id = cartId, Created = DateTime.Now, Expires = DateTime.Now.AddDays(7) };
            _repositoryWrapper.IShoppingCartRepository.Create(cart);
            _repositoryWrapper.Save();
            return cart;
        }

        // POST api/<controller>
        [HttpPost("UpdateShoppingCart")]
        public IActionResult UpdateShoppingCart(ShoppingCartProductDTO apiProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                //find product to add
                Product product = _repositoryWrapper.IProductRepository.GetByCondition(x => x.Id == apiProduct.Id);
                if (product == null)
                {
                    return NotFound();
                }
                //handle shopping cart update
                ShoppingCart cart = GetOrCreateShoppingCart();
                if (cart != null)
                {
                    AddProductToCart(apiProduct, product, cart);
                    return new JsonResult(new ShoppingCartDTO(cart.Id, cart.CartDataJSON));
                }
            }
            catch (Exception e)
            {
                _repositoryWrapper.ILog.Create(new Log { Logtype = LOG_TYPE.Error, Message = e.Message, Timestamp = DateTime.Now });
                _repositoryWrapper.Save();
                return BadRequest(ModelState);
            }
            return BadRequest(ModelState);
        }

        private void AddProductToCart(ShoppingCartProductDTO apiProduct, Product product, ShoppingCart cart)
        {
            var newCartProduct = new ShoppingCartProductDTO() { Id = product.Id, Name = product.Name, Quantity = apiProduct.Quantity, Price = product.Price };
            if (cart.CartDataJSON != null)
            {
                var productsInCart = JsonSerializer.Deserialize<List<ShoppingCartProductDTO>>(cart.CartDataJSON);
                //if we already have the product in cart, increade quantity
                if (productsInCart.FirstOrDefault(x => x.Id == newCartProduct.Id) != null)
                {
                    productsInCart.First(x => x.Id == newCartProduct.Id).Quantity += newCartProduct.Quantity;
                }
                else
                {
                    productsInCart.Add(newCartProduct);
                }
                cart.CartDataJSON = JsonSerializer.Serialize(productsInCart);
            }
            else
            {
                cart.CartDataJSON = JsonSerializer.Serialize(new List<ShoppingCartProductDTO>() { newCartProduct });
            }
            _repositoryWrapper.IShoppingCartRepository.Update(cart);
            _repositoryWrapper.Save();
        }

        // DELETE api/<controller>/5
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ShoppingCart cart = GetOrCreateShoppingCart();
                if (cart.CartDataJSON != null)
                {
                    var productsInCart = JsonSerializer.Deserialize<List<ShoppingCartProductDTO>>(cart.CartDataJSON);
                    var inCartProduct = productsInCart.FirstOrDefault(x => x.Id == id);
                    if(inCartProduct != null)
                    {
                        if(inCartProduct.Quantity > 1)
                        {
                            productsInCart.First(x => x.Id == id).Quantity--;
                        }
                        else
                        {
                            productsInCart.Remove(inCartProduct);
                        }
                    }
                    cart.CartDataJSON = JsonSerializer.Serialize(productsInCart);

                    _repositoryWrapper.IShoppingCartRepository.Update(cart);
                    _repositoryWrapper.Save();
                    return new JsonResult(new ShoppingCartDTO(cart.Id, cart.CartDataJSON));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _repositoryWrapper.ILog.Create(new Log { Logtype = LOG_TYPE.Error, Message = e.Message, Timestamp = DateTime.Now });
                _repositoryWrapper.Save();
            }
            return BadRequest();
        }

        public class ShoppingCartDTO
        {
            public ShoppingCartDTO(string id, string CartDataJson)
            {
                Id = id;
                Products = new List<ShoppingCartProductDTO>();
                if (!string.IsNullOrEmpty(CartDataJson))
                {
                    Products = JsonSerializer.Deserialize<List<ShoppingCartProductDTO>>(CartDataJson);
                }
            }
            public string Id { get; set; }
            public List<ShoppingCartProductDTO> Products { get; set; }
        }

        public class ShoppingCartProductDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}
