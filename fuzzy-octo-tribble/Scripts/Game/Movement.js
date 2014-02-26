FuzzyOctoTribble.MovementConstructor = function (currentMap) {
    var that = {};
    var map = currentMap;

    that.setMap = function (currentMap) {
        map = currentMap;
    }

    that.moveLeft = function () {
        if (FuzzyOctoTribble.Player.x == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x - 1][FuzzyOctoTribble.Player.y].isTraversable) {
            return;
        }
        FuzzyOctoTribble.Player.x -= 1;
        $.ajax("Game/MoveLeft");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveUp = function () {
        if (FuzzyOctoTribble.Player.y == 0 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y - 1].isTraversable) {
            return;
        }
        FuzzyOctoTribble.Player.y -= 1;
        $.ajax("Game/MoveUp");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveRight = function () {
        if (FuzzyOctoTribble.Player.x == map.mapSquares.length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x + 1][FuzzyOctoTribble.Player.y].isTraversable) {
            return;
        }
        FuzzyOctoTribble.Player.x += 1;
        $.ajax("Game/MoveRight");
        FuzzyOctoTribble.Camera.draw();
    }

    that.moveDown = function () {
        if (FuzzyOctoTribble.Player.y == map.mapSquares[0].length - 1 || !map.mapSquares[FuzzyOctoTribble.Player.x][FuzzyOctoTribble.Player.y + 1].isTraversable) {
            return;
        }
        FuzzyOctoTribble.Player.y += 1;
        $.ajax("Game/MoveDown");
        FuzzyOctoTribble.Camera.draw();
    }

    return that;
}