// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".btn-add").on("click", function (){
    var item = $(this).data("itemid");
    $.get("/Home/JSAddToCart/" + item, function (data) {
        console.log("Added item " + item + " to cart.");

        if (data.success){
            var cartItem = $("#qty-" + item)
            cartItem.val(data.count)
        } else {
            console.log(data.error)
        }
    })
});

$(".btn-remove").on("click", function (){
    var item = $(this).data("itemid");
    $.get("/Home/JSRemoveFromCart/" + item, function (data) {
        console.log("Removed item " + item + " from cart.");

        if (data.success){
            var cartItem = $("#qty-" + item)
            cartItem.val(data.count)
        } else {
            console.log(data.error)
        }
    })
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