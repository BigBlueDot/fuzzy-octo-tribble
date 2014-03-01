FuzzyOctoTribble.DialogBox = function (dialogContent) {
    var that = {};
    var dialog = dialogContent
    var displayedText = "";

    var $dialogContainer = $(document.createElement('div'));
    $dialogContainer.addClass('dialog-container text-font');
    var $innerDialog = $(document.createElement('div'));
    $dialogContainer.append($innerDialog);
    var $next = $(document.createElement('div'));
    $next.text('>');
    $next.css('position', 'absolute');
    $next.css('bottom', '0px');
    $next.css('right', '10px');
    $next.css('font-size', '30px');
    $next.hide();
    $dialogContainer.append($next);
    $('.game-window').append($dialogContainer);

    var showDialog = function () {
        $innerDialog.empty();
        var writer = dialog.split(' ');
        var showArrow = false;
        for (var i = 0; i < writer.length; i++) {
            var currentText = $innerDialog.text();
            $innerDialog.text(currentText + ' ' + writer[i]);
            if ($innerDialog.innerHeight() > $dialogContainer.height()) {
                $innerDialog.text(currentText);
                writer.splice(0, i);
                showArrow = true;
                dialog = writer.join(' ');
                break;
            }
        }

        displayedText = $innerDialog.text();

        if (showArrow) {
            $next.show();
        }
        else {
            $next.hide();
            dialog = '';
        }

    }

    var nextDialog = function () {
        if (dialog) {
            showDialog();
        }
        else {
            $dialogContainer.remove();
            $(window).unbind('resize', resizeDialog);
            if (that.onComplete) {
                that.onComplete();
            }
        }
    }

    resizeDialog = function () {
        if (dialog) {
            dialog = displayedText + ' ' + dialog;
        } else {
            dialog = displayedText;
        }

        nextDialog();
    }

    $(window).bind('resize', resizeDialog);

    that.confirm = function () {
        nextDialog();
    }

    showDialog();

    return that;
}