$(document).ready(function () {
    var mc = {
        '-5;-0.2': 'bad-good-factor',
        '-0.2;1': 'normal-good-factor',
        '1;2': 'good-good-factor',
        '2;5.1': 'best-good-factor'
    };

    function between(x, min, max) {
        return x >= min && x < max;
    }

    var first;
    var second;
    var element;

    $('div').each(function (index) {
        element = $(this);

        if (element.attr('data-color') != undefined) {
            var dc = parseFloat(element.attr('data-color').replace(',', '.'));

            $.each(mc, function (name, value) {
                first = parseFloat(name.split(';')[0], 10);
                second = parseFloat(name.split(';')[1], 10);

                if (between(dc, first, second)) {
                    element.addClass(value);
                }
            });
        }
    });
});