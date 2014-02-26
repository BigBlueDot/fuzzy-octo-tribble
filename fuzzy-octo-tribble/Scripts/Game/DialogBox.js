FuzzyOctoTribble.DialogBox = (function () {
    var that = {};
    var dialogActive = false;
    var dialogQueue = [];
    var displayedText = "";

    var $dialogContainer = $(document.createElement('div'));
    $dialogContainer.addClass('dialog-container text-font');
    $dialogContainer.hide();
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

    that.showDialog = function (dialog) {
        if (dialogActive) {
            if (dialog) {
                dialogQueue.push(dialog);
            }
        }
        else {
            dialogActive = true;
            if (dialog) {
                dialogQueue.push(dialog);
            }
            $innerDialog.empty();
            $dialogContainer.show(400, function () {
                FuzzyOctoTribble.KeyControl.setDialogMode();

                var writer = dialogQueue[0].split(' ');
                var showArrow = false;
                for (var i = 0; i < writer.length; i++) {
                    var currentText = $innerDialog.text();
                    $innerDialog.text(currentText + ' ' + writer[i]);
                    if ($innerDialog.innerHeight() > $dialogContainer.height()) {
                        $innerDialog.text(currentText);
                        writer.splice(0, i);
                        showArrow = true;
                        dialogQueue[0] = writer.join(' ');
                        break;
                    }
                }

                displayedText = $innerDialog.text();

                if (showArrow || dialogQueue[1]) {
                    $next.show();
                }
                else {
                    $next.hide();
                    dialogQueue.shift();
                }
            });
        }
    }

    that.nextDialog = function () {
        dialogActive = false;
        if (dialogQueue[0]) {
            that.showDialog();
        }
        else {
            $dialogContainer.hide();
            FuzzyOctoTribble.KeyControl.cancelDialogMode();
        }
    }

    that.resizeDialog = function () {
        if (dialogActive) {
            if (dialogQueue[0]) {
                dialogQueue[0] = displayedText + ' ' + dialogQueue[0];
            } else {
                dialogQueue[0] = displayedText;
            }

            that.nextDialog();
        }
    }

    that.drawDialog = function () {
        $('.game-window').append($dialogContainer);
    }

    $(window).resize(function (e) {
        that.resizeDialog();
    });

    return that;
})();