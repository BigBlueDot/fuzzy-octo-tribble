FuzzyOctoTribble.KeyControlConstructor = function (currentMap) {
    var that = {};
    var timer37, timer38, timer39, timer40;
    var interval = 300;
    var map = currentMap;
    var canMove = true;
    var dialogMode = false;

    var clearTimers = function () {
        clearTimeout(timer37);
        clearTimeout(timer38);
        clearTimeout(timer39);
        clearTimeout(timer40);
    }

    var moveLeft = function () {
        FuzzyOctoTribble.Movement.moveLeft();
        clearTimers();
        timer37 = setTimeout(moveLeft, interval);
    }
    var moveUp = function () {
        FuzzyOctoTribble.Movement.moveUp();
        clearTimers();
        timer38 = setTimeout(moveUp, interval);
    }
    var moveRight = function () {
        FuzzyOctoTribble.Movement.moveRight();
        clearTimers();
        timer39 = setTimeout(moveRight, interval);
    }
    var moveDown = function () {
        FuzzyOctoTribble.Movement.moveDown();
        clearTimers();
        timer40 = setTimeout(moveDown, interval);
    }

    $(document).ready(function () {
        $(document).keydown(function (e) {
            if (e.keyCode == 37 && !timer37 && canMove) { //left
                moveLeft();
            }
            else if (e.keyCode == 38 && !timer38 && canMove) { //up
                moveUp();
            }
            else if (e.keyCode == 39 && !timer39 && canMove) { //right
                moveRight();
            }
            else if (e.keyCode == 40 && !timer40 && canMove) { //down
                moveDown();
            }
        });

        $(document).keyup(function (e) {
            if (e.keyCode == 37) { //left
                clearTimeout(timer37);
                timer37 = false;
            }
            else if (e.keyCode == 38) { //up
                clearTimeout(timer38);
                timer38 = false;
            }
            else if (e.keyCode == 39) { //right
                clearTimeout(timer39);
                timer39 = false;
            }
            else if (e.keyCode == 40) { //down
                clearTimeout(timer40);
                timer40 = false;
            }
            else if (dialogMode && e.keyCode == 90) {
                FuzzyOctoTribble.DialogBox.nextDialog();
            }
            else if (canMove && e.keyCode == 90) {
                FuzzyOctoTribble.InteractionHandler.getInteraction();
            }
        });
    });

    that.setMovementMode = function () {
        canMove = true;
    }

    that.setDialogMode = function (canMoveVar) {
        canMove = canMoveVar;
        dialogMode = true;
    }

    that.cancelDialogMode = function () {
        canMove = true;
        dialogMode = false;
    }

    return that;
}