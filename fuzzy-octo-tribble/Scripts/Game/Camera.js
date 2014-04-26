FuzzyOctoTribble.Camera = (function () {
    var that = {};
    var map;
    var playerCoordinates;

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
    }

    that.setMap = function (newMap) {
        map = newMap;
    }
    that.setPlayer = function (newPlayerCoordinates) {
        playerCoordinates = newPlayerCoordinates;
    }

    that.draw = function () {
        if (map && playerCoordinates) {
            $('.game-map').empty();
            var height = $('.game-map').height();
            var width = $('.game-map').width();
            var centerX = (width / 2) - 20;
            var centerY = (height / 2) - 20;
            var offsetHeight = centerY % 40;
            var offsetWidth = centerX % 40;
            var drawCountHeight = Math.ceil(centerY / 40);
            var drawCountWidth = Math.ceil(centerX / 40);
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
                            drawSquare(map.mapUrl[map.mapSquares[x][y].imageUrl], drawX, drawY);
                        }

                        for (var i = 0; i < map.events.length; i++) {
                            if (map.events[i].x == x && map.events[i].y == y) {
                                drawEvent(drawX, drawY, map.events[i].rewardType);
                            }
                        }
                    }
                    drawY += 40;
                }
                drawY = -offsetHeight;
                drawX += 40;
            }
        }
    }

    $(window).resize(function (e) {
        that.draw();
    });

    return that;
}())