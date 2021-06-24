var rtime;
var timeout = false;
var delta = 500;

$(document).ready(function () {
    $('#carouselwithIndicators').find('.carousel-item img')
        .css('max-height', $('#carouselwithIndicators').css('height'));

    $('#carouselwithIndicators').find('.carousel-item img')
        .css('min-height', $('#carouselwithIndicators').css('height'));
});

$(window).resize(function () {
    rtime = new Date();
    if (timeout === false) {
        timeout = true;
        setTimeout(resizeend, delta);
    }
});

function resizeend() {
    if (new Date() - rtime < delta) {
        setTimeout(resizeend, delta);
    } else {
        timeout = false;
        location.reload();

        $('#carouselwithIndicators').find('.carousel-item img')
            .css('max-height', $('#carouselwithIndicators').css('height'));

        $('#carouselwithIndicators').find('.carousel-item img')
            .css('min-height', $('#carouselwithIndicators').css('height'));
    }
}