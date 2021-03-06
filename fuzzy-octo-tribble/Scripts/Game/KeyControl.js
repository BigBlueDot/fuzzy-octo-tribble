﻿FuzzyOctoTribble.KeyControlConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;
    var controllers = [];

    var currentController = function() {
        return controllers[controllers.length -1];
    }

    $(document).ready(function () {
        $(document).keydown(function (e) {
            if (e.keyCode == 37) { //left
                that.pressLeft();
            }
            else if (e.keyCode == 38) { //up
                that.pressUp();
            }
            else if (e.keyCode == 39) { //right
                that.pressRight();
            }
            else if (e.keyCode == 40) { //down
                that.pressDown();
            }
        });

        $(document).keyup(function (e) {
            if (e.keyCode == 37) { //left
                that.releaseLeft();
            }
            else if (e.keyCode == 38) { //up
                that.releaseUp();
            }
            else if (e.keyCode == 39) { //right
                that.releaseRight();
            }
            else if (e.keyCode == 40) { //down
                that.releaseDown();
            }
            else if (e.keyCode == 90) {
                that.confirm();
            }
            else if (e.keyCode == 77) {
                that.menu();
            }
            else if (e.keyCode == 88) {
                that.cancel();
            }
        });
    });

    that.removeCombat = function () {
        if (controllers.length !== 0) {
            while (controllers[controllers.length - 1].isCombat) {
                if (controllers[controllers.length - 1].close) {
                    controllers[controllers.length - 1].close();
                }
                else {
                    break;
                }
            }
        }
    }

    that.removeWindows = function (windowType) {
        var windowsToRemove = [];
        for (var i = 0; i < controllers.length; i++) {
            if (controllers[i][windowType] && controllers[i][windowType] === true) {
                windowsToRemove.push(i);
            }
        }

        for (var i = windowsToRemove.length - 1; i >= 0; i--) {
            if (controllers[windowsToRemove[i]].close) {
                controllers[windowsToRemove[i]].close();
            }

            controllers.splice(windowsToRemove[i], 1);
        }
    }

    that.addController = function (controller) {
        if (controllers.length !== 0) {
            controllers[controllers.length - 1].hasCurrentControl = false;
            if (controllers[controllers.length - 1].clearCurrentControl) {
                controllers[controllers.length - 1].clearCurrentControl();
            }
        }
        controller.hasCurrentControl = true;
        controllers.push(controller);
        controller.onComplete = function () {
            var lastController = controllers.pop();
            if (lastController.clearCurrentControl) {
                lastController.clearCurrentControl();
            }
            if (controllers.length !== 0) {
                controllers[controllers.length - 1].hasCurrentControl = true;
                if (controllers[controllers.length - 1].onCurrentControl) {
                    controllers[controllers.length - 1].onCurrentControl();
                }
            }
        }
    }

    that.previousIsCombat = function () {
        if (controllers.length >= 3) {
            if (controllers[controllers.length - 2] && controllers[controllers.length - 2].isCombat) {
                return true;
            }
        }

        return false;
    }

    that.pressLeft = function () {
        if (currentController().pressLeft) {
            currentController().pressLeft();
        }
    }

    that.pressUp = function () {
        if (currentController().pressUp) {
            currentController().pressUp();
        }
    }
    
    that.pressRight = function () {
        if (currentController().pressRight) {
            currentController().pressRight();
        }
    }

    that.pressDown = function () {
        if (currentController().pressDown) {
            currentController().pressDown();
        }
    }

    that.releaseLeft = function () {
        if (currentController().releaseLeft) {
            currentController().releaseLeft();
        }
    }

    that.releaseUp = function () {
        if (currentController().releaseUp) {
            currentController().releaseUp();
        }
    }

    that.releaseDown = function () {
        if (currentController().releaseDown) {
            currentController().releaseDown();
        }
    }

    that.releaseRight = function () {
        if (currentController().releaseRight) {
            currentController().releaseRight();
        }
    }

    that.confirm = function () {
        if (currentController().confirm) {
            currentController().confirm();
        }
    }

    that.cancel = function () {
        if (currentController().cancel) {
            currentController().cancel();
        }
    }

    that.menu = function () {
        if (currentController().menu) {
            currentController().menu();
        }
    }
        

    return that;
}