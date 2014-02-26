FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var calcPlayer = function () {
        $.ajax("Game/GetPlayer", {
            success: function (data) {
                FuzzyOctoTribble.Player = data;
                FuzzyOctoTribble.Camera.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.Camera.draw();
                setTimeout(calcPlayer, 5000);
            }
        });
    }

    var calcMap = function () {
        $.ajax("Game/GetMap", {
            success: function (data) {
                FuzzyOctoTribble.Camera.setMap(data);
                FuzzyOctoTribble.Movement.setMap(data);
                setTimeout(calcMap, 5000);
            }
        });
    }

    that.start = function () {
        calcPlayer();
        calcMap();
    }

    return that;
})();