﻿$(document).ready(function () {
    // Set a timeout to hide the error message after 3 seconds
    setTimeout(function () {
        $('#errorMessage').fadeOut('slow');
    }, 3000);

    $('#cart-container').on('mouseenter', function () {
        // Show cart content or any additional information
        $(this).html('<p>Your cart content goes here...</p>');
    });

    $('#cart-container').on('mouseleave', function () {
        // Hide cart content when mouse leaves the cart area
        $(this).html('');
    });

});

ShowSnackBarSuccessMessage = (message) => {

    let snackbar = document.getElementById("snackbar");
    $(snackbar).removeClass();
    $(snackbar).html("<i class=\"fa fa-check-circle\" aria-hidden=\"true\" style=\"margin-right:3px;\"></i>" + message);
    $(snackbar).addClass("alert alert-success show");
    setTimeout(function () { $(snackbar).removeClass("show"); }, 3000);
}

ShowSnackBarFailureMessage = (message) => {

    let snackbar = document.getElementById("snackbar");
    $(snackbar).removeClass();
    $(snackbar).html("<i class=\"fa fa-exclamation-circle\" aria-hidden=\"true\" style=\"margin-right:3px;\"></i>" + message);
    $(snackbar).addClass("alert alert-danger show");
    setTimeout(function () { $(snackbar).removeClass("show"); }, 3000);
}

ShowModelDialogFailureAlert = (message) => {

    let alertContainer = $("#alert-box-app");
    $("#alert-box-app .text-danger").html(message);

    $("#alert-box-app p.text-danger").css("display", "block");
    alertContainer.addClass("show");

    setTimeout(function () {
        alertContainer.removeClass("show");
        $("#alert-box-app p.text-danger").css("display", "none");
    }, 3000);

}