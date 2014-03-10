FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var playerTimer;

    var calcPlayer = function () {
        $.ajax("Game/GetPlayer", {
            success: function (data) {
                var currentParty = [];
                for (var i = 0; i < data.currentPartyCharacters; i++) {
                    for (var j = 0; j < data.characters.length; j++) {
                        if (data.characters[j].uniq === data.currentPartyCharacters[i]) {
                            currentParty.push(data.characters[j]);
                        }
                    }
                }
                
                if (!FuzzyOctoTribble.PlayerDirection) {
                    FuzzyOctoTribble.PlayerDirection = 4;
                }
                FuzzyOctoTribble.Player = data;
                FuzzyOctoTribble.Camera.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.MenuHandler.setPlayer(FuzzyOctoTribble.Player);
                FuzzyOctoTribble.CharacterScreenCreator.setCharacters(FuzzyOctoTribble.Player.characters, currentParty);
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
                FuzzyOctoTribble.MenuHandler.setIsDungeon(data.isDungeon);
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