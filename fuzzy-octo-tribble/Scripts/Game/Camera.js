FuzzyOctoTribble.Camera = (function () {
    var that = {};
    var map;
    var playerCoordinates;
    var squareSize = 64;

    var drawSquare = function (url, x, y) {
        $square = $(document.createElement('div'));
        $square.addClass('map-square');
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
        if (doDraw) {
            that.draw();
        }
    }
    that.setPlayer = function (newPlayerCoordinates) {
        playerCoordinates = newPlayerCoordinates;
        that.draw();
    }

    that.draw = function () {
        if (map && playerCoordinates) {
            $('.game-map').empty();
            var height = $('.game-map').height();
            var width = $('.game-map').width();
            var centerX = (width / 2) - (squareSize / 2);
            var centerY = (height / 2) - (squareSize / 2);
            var offsetHeight = centerY % squareSize;
            var offsetWidth = centerX % squareSize;
            var drawCountHeight = Math.ceil(centerY / squareSize);
            var drawCountWidth = Math.ceil(centerX / squareSize);
            var topIndex = playerCoordinates.y - drawCountHeight;
            var leftIndex = playerCoordinates.x - drawCountWidth;
            var drawX = -offsetWidth;
            var drawY = -offsetHeight;
            
            for (var x = leftIndex; x < (leftIndex) + (drawCountWidth * 2) + 2; x++) {
                for (var y = topIndex; y < (topIndex) + (drawCountHeight * 2) + 2; y++) {
                    if (x >= 0 && y >= 0 && x < map.mapSquares.length && y < map.mapSquares[0].length) {
                        if (x == playerCoordinates.x && y == playerCoordinates.y) {
                            if (!FuzzyOctoTribble.PlayerDirection || FuzzyOctoTribble.PlayerDirection === 4) {
                                drawSquare("/Images/Game/PlayerDown.png", drawX, drawY);
                            }
                            else if (FuzzyOctoTribble.PlayerDirection === 1) {
                                drawSquare("/Images/Game/PlayerLeft.png", drawX, drawY);
                            }
                            else if (FuzzyOctoTribble.PlayerDirection === 2) {
                                drawSquare("/Images/Game/PlayerUp.png", drawX, drawY);
                            }
                            else if (FuzzyOctoTribble.PlayerDirection === 3) {
                                drawSquare("/Images/Game/PlayerRight.png", drawX, drawY);
                            }
                        }
                        else {
                            drawSquare(map.mapUrl[map.mapSquares[x][y].i], drawX, drawY);
                        }

                        for (var i = 0; i < map.events.length; i++) {
                            if (map.events[i].x == x && map.events[i].y == y) {
                                drawEvent(drawX, drawY, map.events[i].rewardType);
                            }
                        }
                    }
                    drawY += squareSize;
                }
                drawY = -offsetHeight;
                drawX += squareSize;
            }
        }
    }

    $(window).resize(function (e) {
        that.draw();
    });

    return that;
}())