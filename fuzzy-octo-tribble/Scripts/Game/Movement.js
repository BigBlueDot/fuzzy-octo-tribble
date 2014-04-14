FuzzyOctoTribble.MovementConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;
    var timer37, timer38, timer39, timer40;
    var interval = 300;

    var clearTimers = function () {
        clearTimeout(timer37);
        clearTimeout(timer38);
        clearTimeout(timer39);
        clearTimeout(timer40);
        timer37 = false;
        timer38 = false;
        timer39 = false;
        timer40 = false;
    }

    var checkSuccess = function (data) {
        if (data === 1) { //Combat
            FuzzyOctoTribble.CombatAccess.startCombat();
        }
    }

    var moveLeft = function () {
        FuzzyOctoTribble.PlayerDirection = 1;
        if (FuzzyOctoTribble.Player.x == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x - 1][FuzzyOctoTribble.Player.y].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.x -= 1;
        $.ajax("Game/MoveLeft", {
            success: checkSuccess
        });
        FuzzyOctoTribble.Camera.draw();
        timer37 = setTimeout(moveLeft, interval);
    }

    var moveUp = function () {
        FuzzyOctoTribble.PlayerDirection = 2;
        if (FuzzyOctoTribble.Player.y == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y - 1].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.y -= 1;
        $.ajax("Game/MoveUp", {
            success: checkSuccess
        });
        FuzzyOctoTribble.Camera.draw();
        timer38 = setTimeout(moveUp, interval);
    }

    var moveRight = function () {
        FuzzyOctoTribble.PlayerDirection = 3;
        if (FuzzyOctoTribble.Player.x == map.mapSquares.length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x + 1][FuzzyOctoTribble.Player.y].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.x += 1;
        $.ajax("Game/MoveRight", {
            success: checkSuccess
        });
        FuzzyOctoTribble.Camera.draw();
        timer39 = setTimeout(moveRight, interval);
    }

    var moveDown = function () {
        FuzzyOctoTribble.PlayerDirection = 4;
        if (FuzzyOctoTribble.Player.y == map.mapSquares[0].length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y + 1].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.y += 1;
        $.ajax("Game/MoveDown", {
            success: checkSuccess
        });
        FuzzyOctoTribble.Camera.draw();
        timer40 = setTimeout(moveDown, interval);
    }

    that.setMap = function (currentMap) {
        map = currentMap;
    }

    that.pressLeft = function () {
        if (!timer37) {
            moveLeft();
            clearTimers();
            timer37 = setTimeout(moveLeft, interval);
        }
    }

    that.pressUp = function () {
        if (!timer38) {
            moveUp();
            clearTimers();
            timer38 = setTimeout(moveUp, interval);
        }
    }

    that.pressRight = function () {
        if (!timer39) {
            moveRight();
            clearTimers();
            timer39 = setTimeout(moveRight, interval);
        }
    }

    that.pressDown = function () {
        if (!timer40) {
            moveDown();
            clearTimers();
            timer40 = setTimeout(moveDown, interval);
        }
    }

    that.releaseLeft = function () {
        clearTimeout(timer37);
        timer37 = false;
    }

    that.releaseUp = function () {
        clearTimeout(timer38);
        timer38 = false;
    }

    that.releaseRight = function () {
        clearTimeout(timer39);
        timer39 = false;
    }

    that.releaseDown = function () {
        clearTimeout(timer40);
        timer40 = false;
    }

    that.confirm = function () {
        FuzzyOctoTribble.InteractionHandler.getInteraction();
    }

    that.cancel = function () {
        //Do nothing for now
    }

    that.menu = function () {
        FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.MenuHandler.getMenu());
    }

    that.clearCurrentControl = function () {
        clearTimers();
    }

    return that;
}