var user = (function () {

    var registerUser = (form) => {

        let registerForm = $("#formRegister")[0];
        var formData = new FormData(registerForm);


        $.ajax({
            url: '/User/RegisterUser',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    alert(res.message);
                    ShowSnackBarSuccessMessage(res.message+' Proceed to Login');
                    window.location.href = '/User/Login';
                }
                else {
                    alert(res.message);
                    ShowSnackBarFailureMessage(res.message);
                }
            },
            error: function (error) {
                console.error(error);
            }
        });
    }

function redirectToRegister() {
    // You can replace '/Account/Register' with the actual URL of your register page
    window.location.href = '/User/Register';
}
    return {
        registerUser,
        redirectToRegister
    }

})();