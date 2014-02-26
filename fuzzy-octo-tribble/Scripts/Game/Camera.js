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
        $('.game-window').append($square);
    }

    that.setMap = function (newMap) {
        map = newMap;
    }
    that.setPlayer = function (newPlayerCoordinates) {
        playerCoordinates = newPlayerCoordinates;
    }

    that.draw = function () {
        if (map && playerCoordinates) {
            $('.game-window').empty();
            var height = $('.game-window').height();
            var width = $('.game-window').width();
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
                            drawSquare("/Images/Game/Player.png", drawX, drawY);
                        }
                        else {
                            drawSquare(map.mapSquares[x][y].imageUrl, drawX, drawY);
                        }
                    }
                    drawY += 40;
                }
                drawY = -offsetHeight;
                drawX += 40;
            }

            FuzzyOctoTribble.DialogBox.drawDialog();
        }
    }

    $(window).resize(function (e) {
        that.draw();
    });

    return that;
}())