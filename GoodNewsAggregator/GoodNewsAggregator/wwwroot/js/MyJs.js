$(document).ready(function () {
    var mc = {
        '0-49': 'bad-good-factor',
        '50-55': 'normal-good-factor',
        '56-100': 'good-good-factor'
    };

    function between(x, min, max) {
        return x >= min && x <= max;
    }

    var dc;
    var first;
    var second;
    var th;

    $('div').each(function (index) {

        th = $(this);

        dc = parseInt($(this).attr('data-color'), 10);

        $.each(mc, function (name, value) {
            first = parseInt(name.split('-')[0], 10);
            second = parseInt(name.split('-')[1], 10);

            if (between(dc, first, second)) {
                th.addClass(value);
            }
        });
    });
});