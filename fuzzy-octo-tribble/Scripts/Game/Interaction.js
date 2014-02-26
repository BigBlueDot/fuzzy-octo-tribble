
FuzzyOctoTribble.InteractionHandler = (function () {
    var that = {};
    var map;

    that.setMap = function (newMap) {
        map = newMap;
    }

    that.getInteraction = function () {
        var x = FuzzyOctoTribble.Player.x;
        var y = FuzzyOctoTribble.Player.y;
        switch (FuzzyOctoTribble.PlayerDirection) {
            case 1:
                x -= 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                x += 1;
                break;
            case 4:
                y += 1;
                break;
            default:
                break;
        }

        if (map) {
            if (x === 0 || y === 0 || x === map.mapSquares.length || y === map.mapSquares[0].length) {
                return;
            }
            else if (!map.mapSquares[x][y].isInteractable) {
                return;
            }
        }
        
        $.ajax("Game/GetInteraction?x=" + x + "&y=" + y, {
            success: function (data) {
                if (data.hasDialog) {
                    FuzzyOctoTribble.DialogBox.showDialog(data.dialog);
                }
            }
        });
    }

    return that;
})();