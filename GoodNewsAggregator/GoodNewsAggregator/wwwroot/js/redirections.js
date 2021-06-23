function myAccount() {
    window.location.href = "/Account/MyAccount";
}

function exit() {
    document.getElementById("exitForm").submit();
}

function login() {
    window.location.href = "/Account/Login";
}

function register() {
    window.location.href = "/Account/Register";
}

function home() {
    window.location.href = "/Home/Index";

    console.log($('#carouselwithIndicators').find('.carousel-item img'));
}