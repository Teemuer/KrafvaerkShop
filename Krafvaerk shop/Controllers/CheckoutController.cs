using System;
using System.Collections.Generic;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Krafvaerk_shop.Models;

namespace Krafvaerk_shop.Controllers
{
    public class CheckoutController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IConfiguration _configuration;
        public CheckoutController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _repositoryWrapper = repositoryWrapper;
        }

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


        [HttpGet]
        public ActionResult Create()
        {
            string cartId = HttpContext.Session.GetString(CartKey);
            if (!string.IsNullOrEmpty(cartId))
            {
                ShoppingCart cart = _repositoryWrapper.IShoppingCartRepository.GetByCondition(x => x.Id == cartId);
                CheckoutViewModel model = new CheckoutViewModel(cart);
                return View(model);
            }
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                Order order = CreateOrderFromModel(model);
                _repositoryWrapper.IOrderRepository.Create(order);

                //remove shopping cart from db
                RemoveShoppingCart(model);

                //save cart remove and order update
                _repositoryWrapper.Save();

                _repositoryWrapper.ILog.Create(new Log { Logtype = LOG_TYPE.Order, Message = "Order placed: orderId:" + order.Id + " UserId: " + order.User.Id, Timestamp = DateTime.Now });
                _repositoryWrapper.Save();

                return RedirectToAction(nameof(Success));
            }
            catch(Exception e)
            {
                _repositoryWrapper.ILog.Create(new Log { Logtype = LOG_TYPE.Error, Message = e.Message, Timestamp = DateTime.Now });
                _repositoryWrapper.Save();
                return View(model);
            }
        }

        private void RemoveShoppingCart(CheckoutViewModel model)
        {
            ShoppingCart cart = _repositoryWrapper.IShoppingCartRepository.GetByCondition(x => x.Id == model.ShoppingCartId);
            if (cart != null)
            {
                _repositoryWrapper.IShoppingCartRepository.Delete(cart);
            }
        }

        private static Order CreateOrderFromModel(CheckoutViewModel model)
        {
            //create new order
            Order order = new Order
            {
                Created = DateTime.Now,
                Status = ORDER_STATUS.Ordered,
                User = model.User,
                OrderRows = new List<OrderRow>()
            };
            //create order rows 
            foreach (var p in model.Products)
            {
                order.OrderRows.Add(new OrderRow
                {
                    ProductId = p.Id,
                    Quantity = p.Quantity
                });
            }
            return order;
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
    }
}