var btnComments = document.getElementById('comments-display-switcher');

var isOpened = false;

function getComments(articleId) {
    if (btnComments != null) {
        if (isOpened) {
            btnComments.innerHTML = 'Посмотреть комментарии';
            document.getElementById('comments-container').innerHTML = '';
        } else {
            btnComments.innerHTML = 'Скрыть комментарии';
            var comments = document.getElementById('comments-container');
            loadComments(articleId, comments);
        }
    }
    isOpened = !isOpened;
}

function loadComments(articleId, comments) {
    var request = XML.XMLHttpRequest();
    request.open('GET', `/Comments/List?articleId=${articleId}`, true);

    request.onload = function() {
        if (request.status >= 200 && request.stat < 400) {
            var response = request.responseText;
            comments.innerHTML = response;
        }
    }

    request.send();
}