FuzzyOctoTribble.MovementConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;

    that.setMap = function (currentMap) {
        map = currentMap;
    }

    that.moveLeft = function () {
        FuzzyOctoTribble.PlayerDirection = 1;
        if (FuzzyOctoTribble.Player.x == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x - 1][FuzzyOctoTribble.Player.y].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.x -= 1;
        $.ajax("Game/MoveLeft");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveUp = function () {
        FuzzyOctoTribble.PlayerDirection = 2;
        if (FuzzyOctoTribble.Player.y == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y - 1].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.y -= 1;
        $.ajax("Game/MoveUp");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveRight = function () {
        FuzzyOctoTribble.PlayerDirection = 3;
        if (FuzzyOctoTribble.Player.x == map.mapSquares.length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x + 1][FuzzyOctoTribble.Player.y].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.x += 1;
        $.ajax("Game/MoveRight");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveDown = function () {
        FuzzyOctoTribble.PlayerDirection = 4;
        if (FuzzyOctoTribble.Player.y == map.mapSquares[0].length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y + 1].isTraversable) {
            FuzzyOctoTribble.Camera.draw();
            return;
        }
        FuzzyOctoTribble.Player.y += 1;
        $.ajax("Game/MoveDown");
        FuzzyOctoTribble.Camera.draw();
    }

    return that;
}