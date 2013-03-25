(function ($, undefined) {
	var initRedirect = function () {
		var redirectUrl = $('a.redirect:first').attr("href");

		if (redirectUrl !== undefined) {
			setTimeout(function () {
				window.location.href = redirectUrl;
			}, 3000);
		}
	};

	$(function () {
		$('[rel=tooltip]').tooltip({
			placement: "right"
		});

		initRedirect();
	});

	$('.send-email').on('click', function () {
		var that = $(this);
		var activityId = that.parents('[data-activity-id]').data('activity-id');
		var sendModal = $('#sendEmailPopup');
		sendModal.data('activity-id', activityId);
		sendModal.modal('show', { backdrop: 'static' });

		refreshPreview();
	});

	$('#refreshPreview').on('click', function () {
		refreshPreview();
	});

	function refreshPreview() {
		var activityId = $('#sendEmailPopup').data('activity-id');
		var freeText = $('#freeText');

		$.ajax({
			url: "/Activity/GetEmailPreview?id=" + activityId + "&text=" + encodeURIComponent(freeText.val()),
			type: "GET"
		})
	.done(function (result) {
		$("#previewSubject").html(result.content.Subject);
		$("#previewArea").html(result.content.Body);
	});
	}

	$('#sendEmail').on('click', function () {
		var that = $(this);
		var modal = $('#sendEmailPopup');
		var failMessage = modal.find('.alert');
		var activityId = modal.data('activity-id');
		var freeText = $('#freeText');
		var buttons = modal.find('button');

		buttons.attr('disabled', 'disabled');
		failMessage.addClass('hidden');

		$.ajax({
			url: '/Activity/SendEmail',
			type: 'POST',
			data: { id: activityId, text: freeText.val() }
		})
            .done(function () {
            	freeText.val("");
            	modal.modal('hide');
            })
            .fail(function () {
            	failMessage.removeClass('hidden');
            })
            .always(function () {
            	buttons.attr('disabled', false);
            });
	});

}(jQuery));