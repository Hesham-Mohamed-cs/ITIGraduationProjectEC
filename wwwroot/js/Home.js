function navigateToProduct(id) {
    var productUrl = `/Home/GetProduct/${id}`;
    window.location.href = productUrl;
}

function AddToCart(itemId) {

    document.getElementById(itemId).innerHTML = "Added";
    document.getElementById(itemId).style.backgroundColor = "Green";
    var existingData = localStorage.getItem('cartItems');


    var cartItems = existingData ? JSON.parse(existingData) : [];


    if (cartItems.includes(itemId)) {
        console.log('Item already added to cart:', itemId);
    }
    else {

        cartItems.push(itemId);


        localStorage.setItem('cartItems', JSON.stringify(cartItems));


        console.log('Item added to cart:', itemId);
        document.getElementById("CartIcon").innerHTML = cartItems.length
    }

}
function GoToCart() {

    var cartItems = localStorage.getItem('cartItems');

    if (cartItems === null || cartItems === undefined || cartItems === '') {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Your Cart is Empty!',
        })

    }

    else {

        $.ajax({

            url: '/Cart/GetIDs',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ ids: cartItems }),
            success: function (response) {
                // Handle the response from the server if needed
                console.log(response);
                window.location.href = '/Cart/Index';
                console.log(cartItems);
                console.log(typeof (cartItems));

            },
            error: function (error) {
                // Handle errors if any
                console.error(error);
            }
        });
    }

}
//function navigateToCart(id) {
//    var CarttUrl = `/Cart/${id}`;
//    window.location.href = CarttUrl;
//}
window.onload = function () {
    // Retrieve cart items from local storage
    var cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];

    document.getElementById("CartIcon").innerHTML = cartItems.length
    // Loop through each item ID in cartItems
    cartItems.forEach(function (itemId) {
        // Get the HTML element with the corresponding itemId
        var element = document.getElementById(itemId);

        // Check if the element exists
        if (element) {
            // Update inner HTML and style
            element.innerHTML = "Added";
            element.style.backgroundColor = "Green";
        }
    });

}



function done() {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: 'Your Order has been received Successfully!',
        showConfirmButton: false,
        timer: 1500
    })
}


function deleteLS() {
    //localStorage.removeItem("cartItems");
    localStorage.setItem("cartItems", [])
    
    
    /*if (element)*/
}
function checkedpaypal() {
    debugger;
    var element = document.getElementById("debit");
    var element2 = document.getElementById("totalPrice").innerHTML;
    
    console.log(element.checked == true)

    if (element.checked == true) {
        window.location.href = `/Cart/PaymentWithPayPal?totalPrice=${element2}`;

        //$.ajax({
        //    url: `/Cart/PaymentWithPayPal`,
        //    type: 'GET',
        //    data: {
        //        totalPrice=element2
        //    }
        //    success: function (data) {
        //        console.log('success loading data.');
        //    },
        //    error: function () {
        //        console.log('Error loading data.');
        //    }
        //});
    }  
}

    //    // Function to clear the value associated with a key in local storage
    //function clearLocalStorageValue(key) {
    //    localStorage.setItem(key, null);
    //    }

    //// Call the function when the page loads
    //window.onload = function() {
    //        // Specify the key of the item you want to clear
    //        var keyToClear = 'yourLocalStorageKey';

    //// Clear the value associated with the key in local storage
    //clearLocalStorageValue(keyToClear);
    //    };




















//////////[ Loading Image ]////////////

//var imgElement = document.getElementById("prodIMG");

//imgElement.addEventListener("load", function () {
//    console.log("Image loaded successfully.");
//    // Perform actions when the image is loaded
//});

//imgElement.addEventListener("error", function () {
//    console.log("Error loading the image.");
//    var loadingDIV = document.getElementById("loading");
//    loadingDIV.innerHTML = '<div class="spinner-border text-warning" role="status"><span class="visually-hidden">Loading...</span></div>';
//    // Perform actions when there's an error loading the image
//});









