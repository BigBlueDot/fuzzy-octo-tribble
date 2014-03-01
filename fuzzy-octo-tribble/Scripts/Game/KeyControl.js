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

    that.addController = function(controller) {
        controllers.push(controller);
        controller.onComplete = function () {
            controllers.pop();
        }
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