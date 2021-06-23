$(document).ready(function () {
    $('#carouselwithIndicators').find('.carousel-item img')
        .css('max-height', $('#carouselwithIndicators').css('height'));

    $('#carouselwithIndicators').find('.carousel-item img')
        .css('min-height', $('#carouselwithIndicators').css('height'));
});