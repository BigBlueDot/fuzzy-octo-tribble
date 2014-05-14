FuzzyOctoTribble.MovementConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;
    var timer37, timer38, timer39, timer40;
    var interval = 300;
    var animationTimer;
    var moveAllowed = true;

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
        else if (data === 2) { //Update message queue
            FuzzyOctoTribble.MaintainState.checkNow();
        }
    }

    var lastTime = Date.now();
    var totalMove = 0;
    var currentMove = 0;
    var horizontal = true;
    var animate = function () {
        var newTime = Date.now();
        var timeChange = newTime - lastTime;
        lastTime = newTime;
        var move = totalMove * (timeChange / 300);
        currentMove += move;
        if (totalMove < 0) {
            if (currentMove <= totalMove) {
                move = move + (currentMove - totalMove);
                moveAllowed = true;
                calcNextAnimation();
                return;
            }
        }
        else {
            if (currentMove >= totalMove) {
                move = move - (currentMove - totalMove);
                moveAllowed = true;
                calcNextAnimation();
                return;
            }
        }
        if (horizontal) {
            FuzzyOctoTribble.Camera.movePlayer(move, 0);
        }
        else {
            FuzzyOctoTribble.Camera.movePlayer(0, move);
        }
        setTimeout(animate, 10);
    }

    var animateLeft = function () {
        var squareSize = FuzzyOctoTribble.Camera.getSquareSize();
        horizontal = true;
        moveAllowed = false;
        lastTime = Date.now();
        totalMove = -squareSize;
        currentMove = 0;
        setTimeout(animate, 10);
    }

    var animateRight = function () {
        var squareSize = FuzzyOctoTribble.Camera.getSquareSize();
        horizontal = true;
        moveAllowed = false;
        lastTime = Date.now();
        totalMove = squareSize;
        currentMove = 0;
        setTimeout(animate, 10);
    }

    var animateUp = function () {
        var squareSize = FuzzyOctoTribble.Camera.getSquareSize();
        moveAllowed = false;
        horizontal = false;
        lastTime = Date.now();
        totalMove = - squareSize;
        currentMove = 0;
        setTimeout(animate, 10);
    }

    var animateDown = function () {
        var squareSize = FuzzyOctoTribble.Camera.getSquareSize();
        moveAllowed = false;
        horizontal = false;
        lastTime = Date.now();
        totalMove = squareSize;
        currentMove = 0;
        setTimeout(animate, 10);
    }

    var calcNextAnimation = function () {
        if (timer37) {
            moveLeft();
        }
        else if (timer38) {
            moveUp();
        }
        else if (timer39) {
            moveRight();
        }
        else if (timer40) {
            moveDown();
        }
    }

    var moveLeft = function () {
        if (!moveAllowed) {
            return;
        }
        FuzzyOctoTribble.PlayerDirection = 1;
        if (FuzzyOctoTribble.Player.x == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x - 1][FuzzyOctoTribble.Player.y].isT) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        animateLeft();
        FuzzyOctoTribble.Player.x -= 1;
        $.ajax("Game/MoveLeft", {
            success: checkSuccess
        });
        timer37 = setTimeout(moveLeft, interval);
    }

    var moveUp = function () {
        if (!moveAllowed) {
            return;
        }
        FuzzyOctoTribble.PlayerDirection = 2;
        if (FuzzyOctoTribble.Player.y == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y - 1].isT) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        animateUp();
        FuzzyOctoTribble.Player.y -= 1;
        $.ajax("Game/MoveUp", {
            success: checkSuccess
        });
        timer38 = setTimeout(moveUp, interval);
    }

    var moveRight = function () {
        if (!moveAllowed) {
            return;
        }
        FuzzyOctoTribble.PlayerDirection = 3;
        if (FuzzyOctoTribble.Player.x == map.mapSquares.length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x + 1][FuzzyOctoTribble.Player.y].isT) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        animateRight();
        FuzzyOctoTribble.Player.x += 1;
        $.ajax("Game/MoveRight", {
            success: checkSuccess
        });
        timer39 = setTimeout(moveRight, interval);
    }

    var moveDown = function () {
        if (!moveAllowed) {
            return;
        }
        FuzzyOctoTribble.PlayerDirection = 4;
        if (FuzzyOctoTribble.Player.y == map.mapSquares[0].length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y + 1].isT) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        animateDown();
        FuzzyOctoTribble.Player.y += 1;
        $.ajax("Game/MoveDown", {
            success: checkSuccess
        });
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

    that.feignMove = function () {
        $.ajax("Game/StandStill", {
            success: checkSuccess
        });
    }

    return that;
}