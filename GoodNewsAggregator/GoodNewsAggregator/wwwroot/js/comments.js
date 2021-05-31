var btnComments = document.getElementById('comments-display-switcher');
var isOpened = false;

function getComments(articleId) {
    if (btnComments != null) {
        if (isOpened) {
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
    request.open('GET', `/Comments/List?articleId=${articleId}`, true);

    request.onload = function () {
        console.log(request.status);
        if (request.status >= 200 && request.status < 400) {
            var response = request.responseText;
            console.log(response);
            comments.innerHTML = response;
            document.getElementById('create-comment-btn').addEventListener("click", createComment);
            document.getElementById('next-comments-btn').addEventListener("click", nextComments);
        }
    }

    request.send();
}

function createComment() {
    var commentText = document.getElementById('commentText').value;
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
            document.getElementById('commentText').value = '';

            var comments = document.getElementById('comments-container');
            loadComments(articleId, comments);
        }
    }
}

function nextComments() {
    alert("HI!");
}