$(document).ready(function () {


    console.log($('#carouselwithIndicators').css('height')); // check the initial height of the carousel;

    // now apply this height as a max-height on all the image items; css will handle the rest;
    $('#carouselwithIndicators').find('.carousel-item img')
        .css('max-height', $('#carouselwithIndicators').css('height'));

    $('#carouselwithIndicators').find('.carousel-item img')
        .css('min-height', $('#carouselwithIndicators').css('height'));
});