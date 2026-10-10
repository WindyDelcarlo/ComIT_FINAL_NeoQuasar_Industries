// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("alpine:init",() => {
        Alpine.data("cart", () => ({
            // SKU, ManufacturerSKU, Image, Quantity, Name, Price, Stock
            items: [],
            taxPercent: 0,
            shipping: -1,
            init(checkOut){
                axios.get("/Home/JSGetCart").then((response) => {
                    console.log(response);
                    if (response.data.success){
                        this.items = response.data.items;
                    }
                    else {
                        this.items = [];
                        console.log("Error fetching cart: " + response.data.error);

                    }
                })
                .catch((error) => {
                    console.log("Error in init:" + error);
                });
                axios.get("/Home/JSCheckTax/").then((response) => {
                    if (response.data.success){
                        console.log("tax registered as " + 100*response.data.tax + "%")
                        this.taxPercent = response.data.tax;
                    }
                    else {
                        console.log(response.data.error);
                        this.taxPercent = 0;
                    }
                })
                .catch((error) => {
                    console.log("Error in init:" + error);
                });
                if (checkOut){
                    this.shipping = 0;
                }
            },
            add(item){
                console.log("Request sent to /Home/JSAddToCart/" + item.sku)
                axios.get("/Home/JSAddToCart/" + item.sku).then((response) => {
                    if (response.data.success){
                        item.quantity++;
                    }
                    else {
                        console.log(response.data.error);
                    }
                })
            },
            remove(item){
                axios.get("/Home/JSRemoveFromCart/" + item.sku).then((response) => {
                    if (response.data.success){
                        item.quantity--;
                        if (item.quantity <= 0) {
                            this.items = items.filter(i => i.sku !== item.sku);
                        }
                    }
                    else {
                        console.log(response.data.error);
                    }
                })
            },
            get subtotal(){
                return this.items.reduce((sum,item) => sum + item.price * item.quantity, 0);
            },
            get shippingCost(){
                if (this.shipping == 0){
                    return 50
                }
                else if (this.shipping == 1){
                    return 200
                }
                else if (this.shipping == 2){
                    return 1000
                }
                else return 0
            },
            get tax(){
                console.log("tax percent is " + 100*this.taxPercent);
                return this.subtotal * this.taxPercent;
            },
            get total(){
            if (Number.isFinite(this.tax))
                return this.subtotal + this.tax + this.shippingCost;
            }
        })),
        Alpine.data("storefront",() => ({
            // sku, image, description, name, manufacturer, manufacturerSKU, price, cart, stock
            searchResults: [],
            init(results){
                this.searchResults = results;
                console.log(this.searchResults);
            },
            add(item){
                console.log("Request sent to /Home/JSAddToCart/" + item.sku)
                axios.get("/Home/JSAddToCart/" + item.sku).then((response) => {
                    if (response.data.success){
                        item.cart++;
                    }
                    else {
                        console.log(response.data.error);
                    }
                })
            },
            remove(item){
                axios.get("/Home/JSRemoveFromCart/" + item.sku).then((response) => {
                    if (response.data.success){
                        if (item.cart > 0) {
                            item.cart--;
                        }
                    }
                    else {
                        console.log(response.data.error);
                    }
                })
            }
        }))
});

$(".role-select").on("change", function(){
    var user = $(this).data("userid");
    var role = $(this).val();
    console.log("role is " + role + "and id is " + user );

    $.get("/Home/JSChangeUserRole?userid=" + user + "&role=" + role, function(data){
        console.log();
        if (data.success){
            console.log("Role edited successfully!")
        } else {
            console.log(data.error);
        }
    })
})

let deleteCardId = null;

$(".btn-direction").on("change",function(){
   $(".list-incoming").toggle();
   $(".list-outgoing").toggle(); 
});

$("#deleteCardModal").on("show.bs.modal", function (event) {
    var button = $(event.relatedTarget);
    var cardId = button.data("id");
    var modal = $(this);
    deleteCardId = cardId;
});

$(".btn-delete-card").on("click",function(){
    if (deleteCardId != null){
        $.get("/Account/JSDeleteCard/" + deleteCardId, function(data){
            if (data.success){
                window.location.href = "/Account/AccountDetails";
            }
            else {
                console.log(data.error);
            }

        });
    }
});

$(".card-select").on("click", function(){
    var cardId = $(this).data("id");
    $.get("/Account/JSSelectCard/" + cardId, function(data){
        if (data.success){
            window.location.href = "/Account/AccountDetails";
        }
        else {
            console.log(data.error);
        }
    })
});

$(".btn-delete-receipt").on("click",function(){
    var receipt = $(this).data("id");
    $.get("/Home/JSReceiptDelete/" + receipt, function(data){
        if (data.success){
            window.location.href = "/Home/ReceiptHistory";
        }
        else {
            console.log(data.error);
        }
    })
})

function capitalize(str){
    return str.charAt(0).toUpperCase() + str.slice(1);
}