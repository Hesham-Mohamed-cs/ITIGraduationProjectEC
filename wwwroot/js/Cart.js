window.onload = function () { CountTotalPrice() }


function showConfirmation(id) {
    // Use a built-in confirmation dialog

    Swal.fire({
        title: 'Are you sure?',
        text: `you want to delete this order`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            RemoveProduct(id);
        }
    })


    // If the user clicks "OK" in the confirmation dialog, call the RemoveProduct function

    // If the user clicks "Cancel," do nothing
}









function RemoveProduct(productId) {
    // Get the current items from local storage
    var cartItems = JSON.parse(localStorage.getItem("cartItems"));

    // Check if cartItems is null or undefined
    if (!cartItems) {
        console.log("Cart is empty.");
        return;
    }

    // Convert productId to a number
    productId = Number(productId);

    // Filter out all occurrences of the product with the given ID
    var updatedCart = cartItems.filter(item => item !== productId);

    // Check if any items were removed
    if (updatedCart.length < cartItems.length) {
        // Update the local storage with the modified array
        localStorage.setItem("cartItems", JSON.stringify(updatedCart));

        console.log("Product with ID " + productId + " has been removed.");
    } else {
        console.log("Product with ID " + productId + " not found in the cart.");
    }
    GoToCart();

}




function increment(itemId, productCount) {
    var countElement = document.getElementById('count_' + itemId);
    var count = parseInt(countElement.innerHTML, 10);

    if (count < productCount) {
        count++;
        updateCount(itemId, count);
        CountTotalPrice(); // Call CountTotalPrice after updating the count
    }
}

function decrement(itemId, productCount) {
    var countElement = document.getElementById('count_' + itemId);
    var count = parseInt(countElement.innerHTML, 10);

    if (count > 1) {
        count--;
        updateCount(itemId, count);
        CountTotalPrice(); // Call CountTotalPrice after updating the count
    }
}

function updateCount(itemId, count) {
    var countElement = document.getElementById('count_' + itemId);
    countElement.innerHTML = count;
}

