// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

//import { Modal } from "../lib/bootstrap/dist/js/bootstrap.bundle";

// for details on configuring this project to bundle and minify static web assets.
//initialize shoppingCart on every request

function ProductDTO(id, name, quantity, price){
    this.Id = id;
    this.Name = name;
    this.Quantity = quantity;
    this.Price = price;
}
var ShoppingCart = {
    apiUri: '/api/ShoppingCartApi',
    cart: null,
    Initialize: () => {
        ShoppingCart.TryGetCartFromServer();
    },
    TryGetCartFromServer: () => {
        ShoppingCart.HandleRequest(ShoppingCart.apiUri + '/GetShoppingCart', 'GET', null);
    },
    SetProductToCart: (id, quantity) => {
        let newProduct = new ProductDTO(id, "", quantity, 0);
        ShoppingCart.HandleRequest(ShoppingCart.apiUri + '/UpdateShoppingCart', 'POST', newProduct, true);
    },
    RemoveProductFromCart: (id) => {
        ShoppingCart.HandleRequest(ShoppingCart.apiUri + '/Delete/' + id, 'DELETE', null);
    },
    HandleRequest: (apiUri, method, product, showCartUpdatedMessage) => {
        let options = {
            method: method,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: method === 'POST' ? JSON.stringify(product) : null //only post has body
        };
        fetch(apiUri, options)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                else {
                    console.log("error setting cart data." + response.status);
                }
            }).then((cart) => {
                if (cart) {
                    return ShoppingCart.RefreshCartUI(cart);
                }
            }).then(() => {
                if (showCartUpdatedMessage) {
                    $("#shoppingCartModal").modal();
                }
            });
    },
    RefreshCartUI: (products) => {
        let modal = $('#shoppingCartModal .modal-body')[0];
        if (modal) {
            //build cart html as string
            if (products.products) {
                $(modal).empty();
                if (products.products.length > 0) {
                    var cartCounter = 0;
                    let cartHtml = '<table class="table table-striped">';
                    cartHtml += '<thead><tr><th>Name</th><th>Quantity</th><th colspan="2">Price</th></tr></thead><tbody>';
                    for (var i = 0; i < products.products.length; i++) {
                        let p = products.products[i];
                        cartHtml += '<tr>';
                        cartHtml += '<td>' + p.name + '</td>';
                        cartHtml += '<td>' + p.quantity + '</td>';
                        cartHtml += '<td>' + p.price + '€ </td>';
                        cartHtml += '<td><a data-value="' + p.id + '" class="remove-item" href="#">Delete</a></td>';
                        cartHtml += '</tr>';
                        cartCounter += p.quantity;
                    }
                    cartHtml += '</tbody></table>';
                    $(modal).append(cartHtml);
                    //lets add count to cart button
                    $('#cart').attr('data-count', cartCounter);
                    $('#cart').addClass('hasProducts');

                    $('#shoppingCartModal .remove-item').click(function (e) {
                        e.preventDefault();
                        ShoppingCart.RemoveProductFromCart($(this).data("value"));
                    });
                }
                else {
                    let cartHtml = '<p>Shopping cart is empty</p>';
                    $(modal).append(cartHtml);
                    $('#cart').removeAttr('data-count');
                    $('#cart').removeClass('hasProducts');
                }
            }
        }
    },
}

//initialize shoppingcart
ShoppingCart.Initialize();
