FuzzyOctoTribble.Camera = (function () {
    var that = {};
    var map;
    var playerCoordinates;
    var squareSize = 64;

    var px, py, cx, cy, bx, by, bxr, byr, MapIndexX, MapIndexY, MapWidth, MapHeight;

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

        //bx and by represent the upper left-hand point for the bounding box
        bx = cx + Math.floor(width / 4);
        by = cy + Math.floor(height / 4);
        bxr = cx + Math.floor(3 * width / 4) - squareSize;
        byr = cy + Math.floor(3 * height / 4) - squareSize;
        
        calcValues();
    }

    var checkCamera = function () {
        //Check to see if the camera should move
        if (px < bx) {
            cx -= (bx - px);
            bxr -= (bx - px);
            bx = px;
            calcValues();
        } else if(px > bxr) {
            cx += (px - bxr);
            bx += (px - bxr);
            bxr = px;
            calcValues();
        }

        if (py < by) {
            cy -= (by - py);
            byr -= (by - py);
            by = py;
            calcValues();
        }
        else if (py > byr) {
            cy += (py - byr);
            by += (py - byr);
            byr = py;
            calcValues();
        }

        
    }

    var calcValues = function () {
        var height = $('.game-map').height();
        var width = $('.game-map').width();

        //MapIndexX and MapIndexY are the indices of the top left most visible block
        var previousX = MapIndexX;
        var previousY = MapIndexY;
        MapIndexX = Math.floor(cx / squareSize);
        MapIndexY = Math.floor(cy / squareSize);

        //MapWidth and MapHeight are the width and height using "Map Squares" as the unit
        MapWidth = (Math.floor((width) / squareSize)) + 3;
        MapHeight = (Math.floor((height) / squareSize)) + 3;

        that.draw();
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
            //Recalculate px, py if it's outside a certain threshold
            if (Math.abs((playerCoordinates.x * squareSize) - px) > squareSize ||
                Math.abs((playerCoordinates.y * squareSize) - py) > squareSize) {
                px = playerCoordinates.x * squareSize;
                py = playerCoordinates.y * squareSize;

                checkCamera();
            }
        }
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
                        (x * squareSize) - cx,
                        (y * squareSize) - cy);
                }
            }

            //Draw events
            for (var i = 0; i < map.events.length; i++) {
                var currentEvent = map.events[i];
                if (currentEvent.x >= MapIndexX && currentEvent.x <= MapIndexX + MapWidth
                    && currentEvent.y >= MapIndexY && currentEvent.y <= MapIndexY + MapHeight) {
                    drawEvent((currentEvent.x * squareSize) - cx,
                        (currentEvent.y * squareSize) - cy,
                        currentEvent.rewardType);
                }
            }

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
        removePlayer();
        init();
    });

    return that;
}())