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
    //console.log(isOpened);
}

function loadComments(articleId, comments) {
    var request = new XMLHttpRequest();
    request.open('GET', `/Comments/List?articleId=${articleId}`, true);

    request.onload = function() {
        if (request.status >= 200 && request.stat < 400) {
            var response = request.responseText;
            comments.innerHTML = response;

            document.getElementById('create-comment-btn').addEventListener("click", createComment);
        }
    }

    request.send();
}
/*
function createComment() {
    var commentText = document.getElementById('commentText').value;
    var newsId = document.getElementById('newsId').value;

    //validateCommentData();

    var postRequest = new XMLHttpRequest();
    postRequest.open("POST", '/Comments/Create', true);
    postRequest.setRequestHeader('Content-Type', 'application/json');

    //let requestData = new {
    //    commentText: commentText
    //}

    postRequest.send(JSON.stringify({
        commentText: commentText,
        newsId: newsId
    }));

    postRequest.onload = function () {
        if (postRequest.status >= 200 && postRequest.status < 400) {
            document.getElementById('commentText').value = '';

            //commentsContainer.innerHTML += '';

            loadComments(newsId);
        }
    }
}
*/