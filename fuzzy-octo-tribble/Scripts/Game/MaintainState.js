FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var calcPlayer = function () {
        $.ajax("Game/GetPlayer", {
            success: function (data) {
                if (!FuzzyOctoTribble.PlayerDirection) {
                    FuzzyOctoTribble.PlayerDirection = 4;
                }
                FuzzyOctoTribble.Player = data;
                FuzzyOctoTribble.Camera.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.CharacterScreen.setCharacters(FuzzyOctoTribble.Player.characters);
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
                FuzzyOctoTribble.InteractionHandler.setMap(data);
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