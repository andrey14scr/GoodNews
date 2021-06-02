var btnComments = document.getElementById('comments-display-switcher');
var isOpened = false;
var isAdd = true;
var next = 0;

function getComments(articleId) {
    if (btnComments != null) {
        if (isOpened) {
            next = 0;

            btnComments.innerHTML = 'Посмотреть комментарии';
            btnComments.classList.remove('btn-outline-primary');
            btnComments.classList.add('btn-primary');

            document.getElementById('comments-container').innerHTML = '';
        } else {
            btnComments.innerHTML = 'Скрыть комментарии';
            btnComments.classList.remove('btn-primary');
            btnComments.classList.add('btn-outline-primary');

            var comments = document.getElementById('comments-container');
            loadComments(articleId, comments);
        }

        btnComments.blur();
    }

    isOpened = !isOpened;
}

function loadComments(articleId, comments) {
    var request = new XMLHttpRequest();
    request.open('GET', `/Comments/List?articleId=${articleId}&next=${next}&add=${isAdd}`, true);
    console.log(next);
    console.log(isAdd);
    isAdd = true;

    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            var response = request.responseText;
            comments.innerHTML += response;
        }
    }

    request.send();
}

function addComment() {
    var textField = document.getElementById('commentText');
    var error = document.getElementById('error-text');
    var commentText = textField.value;

    if (!commentText || commentText.length < 3) {
        error.innerText = '*Комментарий не должен быть пустым и минимум 3 символа';
        return;
    }
    error.innerText = '';

    var articleId = document.getElementById('articleId').value;

    var postRequest = new XMLHttpRequest();
    postRequest.open("POST", '/Comments/Create', true);
    postRequest.setRequestHeader('Content-Type', 'application/json');

    postRequest.send(JSON.stringify({
        Text: commentText,
        ArticleId: articleId
    }));

    postRequest.onload = function () {
        if (postRequest.status >= 200 && postRequest.status < 400) {
            textField.value = '';

            if (isOpened) {
                isAdd = false;
                var comments = document.getElementById('comments-container');
                comments.innerHTML = '';
                isOpened = false;
                getComments(articleId);
            }
        }
    }
}

function nextComments(articleId) {
    next++;

    var nextBtn = document.getElementById('next-comments-btn');
    if (nextBtn != null) {
        nextBtn.remove();
    }

    var comments = document.getElementById('comments-container');
    loadComments(articleId, comments);
}