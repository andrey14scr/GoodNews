$(document).ready(function () {
    var mc = {
        '0-19': 'bad-good-factor',
        '20-59': 'normal-good-factor',
        '60-100': 'good-good-factor'
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