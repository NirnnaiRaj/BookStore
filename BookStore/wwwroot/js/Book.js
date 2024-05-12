
var book = (function () {

    var viewDetails=(bookId)=> {
        // Redirect to the details page with the book ID
        window.location.href = '/Books/BookDetails?id=' + bookId;
    }

    var addToCart = (bookId,bookStatus) => {
        // Check if the book is available
        if (bookStatus === 'True') {
            // Call the AddToCart action in the controller using AJAX
            $.ajax({
                url: '/Books/AddToCart?id=' + bookId,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        // Show a success message using a popup
                        ShowSnackBarSuccessMessage('Book added to cart successfully!');
                    } else {
                        // Show an error message using a popup
                        ShowSnackBarFailureMessage('Failed to add book to cart.');
                    }
                },
                error: function (xhr, status, error) {
                    // Handle AJAX errors
                    console.error('Error adding book to cart:', error);
                }
            });
        } else {
            // Book is not available, show a message
            ShowSnackBarFailureMessage('This item is not available for purchase.');
        }
    }

    var adjustQuantity = (orderID, bookID, status) => {
        var data = {
            Id : orderID,
            Books : {
                Id : bookID
            },
            Type: status
        };
        $.ajax({
            url: '/Books/RemoveFromCart',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    ShowSnackBarSuccessMessage('Book quantity adjusted.');
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                } else {
                    ShowSnackBarFailureMessage('Failed to adjust Quantity.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error adjusting Quantity:', error);
            }
        });
    }
    var proceedToCheckout = () => {
        var itemIds = [];
        var bookIds = [];
        $('.itemId').each(function () {
            var itemId = $(this).text(); // Retrieve the text of the hidden input field
            itemIds.push(itemId);
        });
        $('.bookId').each(function () {
            var bookId = $(this).text(); // Retrieve the text of the hidden input field
            bookIds.push(bookId);
        });

        // Create BooksModel objects with both the ItemId and BookId properties
        var bookList = [];
        // Ensure both arrays are of equal length
        var minLength = Math.min(itemIds.length, bookIds.length);
        for (var i = 0; i < minLength; i++) {
            var item = {
                Id: itemIds[i],
                Book: { Id: bookIds[i] }
            };
            bookList.push(item);
        }

        var checkOutModel = {
            BookLists: bookList, // Assign the array of BooksModel objects to BookLists
        };

        if (bookList.length > 0) {
            $.ajax({
                url: '/Books/ProceedToCheckout',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(checkOutModel),
                success: function (response) {
                    if (response.success) {
                        ShowSnackBarSuccessMessage('Checkout Completed');
                        setTimeout(function () {
                            window.location.href = '/Books/Index';
                        }, 1000);
                    } else {
                        ShowSnackBarFailureMessage('Checkout Failed try again Later.');
                    }
                },
                error: function (xhr, status, error) {
                    // Handle AJAX errors
                    console.error('Error proceeding to checkout:', error);
                }
            });
        } else {
            console.error('No books in the shopping cart.');
        }
    }





    return {
        viewDetails,
        addToCart,
        adjustQuantity,
        proceedToCheckout
    }

})();