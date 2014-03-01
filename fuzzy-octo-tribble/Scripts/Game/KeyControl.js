FuzzyOctoTribble.KeyControlConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;
    var controllers = [];

    var currentController = function() {
        return controllers[controllers.length -1];
    }

    $(document).ready(function () {
        $(document).keydown(function (e) {
            if (e.keyCode == 37) { //left
                if (currentController().pressLeft) {
                    currentController().pressLeft();
                }
            }
            else if (e.keyCode == 38) { //up
                if (currentController().pressUp) {
                    currentController().pressUp();
                }
            }
            else if (e.keyCode == 39) { //right
                if (currentController().pressRight) {
                    currentController().pressRight();
                }
            }
            else if (e.keyCode == 40) { //down
                if (currentController().pressDown) {
                    currentController().pressDown();
                }
            }
        });

        $(document).keyup(function (e) {
            if (e.keyCode == 37) { //left
                if (currentController().releaseLeft) {
                    currentController().releaseLeft();
                }
            }
            else if (e.keyCode == 38) { //up
                if (currentController().releaseUp) {
                    currentController().releaseUp();
                }
            }
            else if (e.keyCode == 39) { //right
                if (currentController().releaseRight) {
                    currentController().releaseRight();
                }
            }
            else if (e.keyCode == 40) { //down
                if (currentController().releaseDown) {
                    currentController().releaseRight();
                }
            }
            else if (e.keyCode == 90) {
                if (currentController().confirm) {
                    currentController().confirm();
                }
            }
            else if (e.keyCode == 77) {
                if (currentController().menu) {
                    currentController().menu();
                }
            }
            else if (e.keyCode == 88) {
                if (currentController().cancel) {
                    currentController().cancel();
                }
            }

            if (false) {
                if (optionDialogMode && e.keyCode === 38) {
                    FuzzyOctoTribble.OptionDialog.selectUp();
                }
                else if (optionDialogMode && e.keyCode === 40) {
                    FuzzyOctoTribble.OptionDialog.selectDown();
                }
                else if (optionDialogMode && e.keyCode === 90) {
                    FuzzyOctoTribble.OptionDialog.selectCurrent();
                }
                else if (optionDialogMode && e.keyCode == 88) {
                    FuzzyOctoTribble.OptionDialog.cancel();
                }

                if (dialogMode && e.keyCode == 90) {
                    FuzzyOctoTribble.DialogBox.nextDialog();
                }

                if (menuMode && e.keyCode === 38) {
                    FuzzyOctoTribble.Menu.selectUp();
                }
                else if (menuMode && e.keyCode === 40) {
                    FuzzyOctoTribble.Menu.selectDown();
                }
                else if (menuMode && e.keyCode === 90) {
                    FuzzyOctoTribble.Menu.selectCurrent();
                }
                else if (menuMode && e.keyCode == 88) {
                    FuzzyOctoTribble.Menu.cancel();
                }
            }
        });
    });

    that.addController = function(controller) {
        controllers.push(controller);
        controller.onComplete = function () {
            controllers.pop();
        }
    }

    return that;
}