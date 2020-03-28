var commentsModule = (function () {
    function handleSuccess() {
        let commentsCount = parseInt($('#comments #comments-count').html(), 10);
        commentsCount += 1;

        $('#comments #comments-count').html(commentsCount);
        $('#addComment textarea').val('');
    }

    function handleError(xhr) {
        alert(xhr.responseText);
    }

    function setNextId() {
        if (parseInt($('#comments-count').html(), 10) === $('.card[data-comment-id]').length) {
            $('#viewMoreComments').hide();
        }
        else {
            let lastCommentId = $('#lastCommentId').val();
            let id = $('.card').last().attr('data-comment-id');

            if (lastCommentId != id) {
                let attributes = $('#viewMoreComments').attr('data-ajax-url').split('?');
                let newsId = $('#newsIdHidden').val();
                let url = attributes[0] + `?id=${id}&newsId=${newsId}`;
                $('#viewMoreComments').attr('data-ajax-url', url);
                $('#lastCommentId').val(id)
            }
        }
    }

    $('#comments').on('click', '.delete-commen-option', function () {
        let self = $(this);
        let id = $(this).parents('.card[data-comment-id]').attr('data-comment-id');

        $.post('/Comments/Delete', { id: id })
            .done(function (isDeleted) {
                if (isDeleted) {
                    let parent = self.parents('.card');
                    parent.remove();

                    let commentsCount = parseInt($('#comments #comments-count').html(), 10);
                    commentsCount -= 1;

                    $('#comments #comments-count').html(commentsCount)
                }
            })
            .fail(function (data) {
                alert(data.responseText);
            });
    });

    $('#comments').on('click', '.update-commen-option', function () {
        let id = $(this).parents('.card[data-comment-id]').attr('data-comment-id');
        $('#commentIdUpdateModal').val(id);

        let content = $(this).parents('.card').find('.card-body .card-text:eq(1)').html();
        $('#commentComentUpdateModal').val(content);

        $('#commentsUpdateModal').modal('toggle');
    });

    $('#saveUpdatedComment').on('click', function () {
        $('#commentsUpdateModal').modal('toggle');

        let data = {
            id: $('#commentIdUpdateModal').val(),
            content: $('#commentComentUpdateModal').val()
        };

        $.post('/Comments/Update', data)
            .done(function (isUpdated) {
                if (isUpdated) {
                    $('.card[data-comment-id=' + data.id + '] .card-text:eq(1)').html(data.content);
                }
            })
            .fail(function (data) {
                alert(data.responseText);
            });
    });

    return {
        handleSuccess: handleSuccess,
        handleError: handleError,
        setNextId: setNextId
    };
}());