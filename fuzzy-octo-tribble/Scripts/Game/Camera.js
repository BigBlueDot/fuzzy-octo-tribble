FuzzyOctoTribble.Camera = (function () {
    var that = {};
    var map;
    var playerCoordinates;
    var squareSize = 64;

    var px, py, cx, cy, tx, ty, MapIndexX, MapIndexY, MapWidth, MapHeight;

    var init = function () {
        var height = $('.game-map').height();
        var width = $('.game-map').width();
        
        var centerX = (width / 2) - (squareSize / 2);
        var centerY = (height / 2) - (squareSize / 2);
        //px and py are the player offsets relative to the screen
        px = playerCoordinates.x * squareSize;
        py = playerCoordinates.y * squareSize;

        //cx and cy are the camera offsets for the upper left-hand point
        cx = px - centerX;
        cy = py - centerY;
        
        calcValues();
    }

    var checkCamera = function () {
        //Check to see if the camera should move
    }

    var calcValues = function () {
        var height = $('.game-map').height();
        var width = $('.game-map').width();

        //tx and ty represent the offset of tiles being displayed based loosely on window size
        tx = cx % squareSize;
        ty = cy % squareSize;

        //MapIndexX and MapIndexY are the indices of the top left most visible block
        MapIndexX = Math.floor(cx / squareSize);
        MapIndexY = Math.floor(cy / squareSize);

        //MapWidth and MapHeight are the width and height using "Map Squares" as the unit
        MapWidth = (Math.floor((width + tx) / squareSize)) + 1;
        MapHeight = (Math.floor((height + ty) / squareSize)) + 1;
    }

    var drawSquare = function (url, x, y, classes) {
        $square = $(document.createElement('div'));
        $square.addClass('map-square');
        if (classes) {
            $square.addClass(classes);
        }
        $square.css({
            'top': y + 'px',
            'left': x + 'px',
            'background-image': 'url(' + url + ")"
        });
        $('.game-map').append($square);
    }

    var drawEvent = function (x, y, rewardType) {
        if (rewardType == 0) {
            drawSquare('/Images/Game/Map/Objective.png', x, y);
        }
        if (rewardType == 1) {
            drawSquare('/Images/Game/Map/ObjectiveComplete.png', x, y);
        }
        else if (rewardType == 2) {
            drawSquare('/Images/Game/Map/GpEvent.png', x, y);
        }
        else if (rewardType == 3) {
            drawSquare('/Images/Game/Map/XpEvent.png', x, y);
        }
        else if (rewardType == 4) {
            drawSquare('/Images/Game/Map/CpEvent.png', x, y);
        }
        else if (rewardType == 5) {
            drawSquare('/Images/Game/Map/GpEvent.png', x, y);
        }
    }

    that.setMap = function (newMap, doDraw) {
        map = newMap;
        if (!px && playerCoordinates) {
            init();
        }
        if (doDraw) {
            that.draw();
        }
    }

    that.setPlayer = function (newPlayerCoordinates) {
        playerCoordinates = newPlayerCoordinates;
        if (!px && map) {
            init();
        }
        else {
            //Recalculate px, py
            px = playerCoordinates.x * squareSize;
            py = playerCoordinates.y * squareSize;

            checkCamera();
        }
        that.draw();
    }

    that.movePlayer = function(deltaX, deltaY) {
        px += deltaX;
        py += deltaY;
        checkCamera();
        removePlayer();
        drawPlayer();
    }

    that.draw = function () {
        if (map && playerCoordinates) {
            $('.game-map').empty();

            var maxX = (MapIndexX + MapWidth < map.mapSquares.length ? MapIndexX + MapWidth : map.mapSquares.length);
            var maxY = (MapIndexY + MapHeight < map.mapSquares[0].length ? MapIndexY + MapHeight : map.mapSquares[0].length);
            var startX = (MapIndexX >= 0 ? MapIndexX : 0);
            var startY = (MapIndexY >= 0 ? MapIndexY : 0);

            //Draw normal squares
            for (var x = startX; x < maxX; x++) {
                for (var y = startY; y < maxY; y++) {
                    drawSquare(map.mapUrl[map.mapSquares[x][y].i],
                        (x * squareSize) - cx - tx,
                        (y * squareSize) - cy - ty);
                }
            }

            //Draw events

            drawPlayer();

        }
    }
    
    var removePlayer = function () {
        $('.player-square').remove();
    }

    var drawPlayer = function () {
        if (!FuzzyOctoTribble.PlayerDirection || FuzzyOctoTribble.PlayerDirection === 4) {
            drawSquare("/Images/Game/Map/PlayerDown.png", px - cx, py - cy, "player-square");
        }
        else if (FuzzyOctoTribble.PlayerDirection === 1) {
            drawSquare("/Images/Game/Map/PlayerLeft.png", px - cx, py - cy, "player-square");
        }
        else if (FuzzyOctoTribble.PlayerDirection === 2) {
            drawSquare("/Images/Game/Map/PlayerUp.png", px - cx, py - cy, "player-square");
        }
        else if (FuzzyOctoTribble.PlayerDirection === 3) {
            drawSquare("/Images/Game/Map/PlayerRight.png", px - cx, py - cy, "player-square");
        }
    }

    that.getSquareSize = function () {
        return squareSize;
    }

    $(window).resize(function (e) {
        init();
        that.draw();
    });

    return that;
}())