FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var playerTimer;

    var calcPlayer = function () {
        $.ajax("Game/GetPlayer", {
            success: function (data) {
                if (!FuzzyOctoTribble.PlayerDirection) {
                    FuzzyOctoTribble.PlayerDirection = 4;
                }
                FuzzyOctoTribble.Player = data;
                FuzzyOctoTribble.Camera.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.MenuHandler.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.CharacterScreenCreator.setCharacters(FuzzyOctoTribble.Player.characters);
                FuzzyOctoTribble.Camera.draw();
                playerTimer = setTimeout(calcPlayer, 5000);
            }
        });
    }

    var calcMap = function () {
        $.ajax("Game/GetMap", {
            success: function (data) {
                FuzzyOctoTribble.Camera.setMap(data);
                FuzzyOctoTribble.Movement.setMap(data);
                FuzzyOctoTribble.InteractionHandler.setMap(data);
            }
        });
    }

    that.start = function () {
        calcPlayer();
        calcMap();
    }

    that.updateMap = function () {
        clearTimeout(playerTimer);
        calcMap();
        calcPlayer();
    }

    return that;
})();