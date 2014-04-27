FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var playerTimer;

    var periodicCheck = function () {
        calcPlayer(function () {
            calcMessage(function () {
                FuzzyOctoTribble.Camera.draw();
                maintainTimer = setTimeout(periodicCheck, 5000);
            });
        });
    }

    var calcMessage = function (success) {
        $.ajax("Game/getMessages", {
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    switch (data[i].type) {
                        case 0:
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox({ dialogContent: data[i].message }));
                            break;
                        case 1:
                            calcMap();
                            break;
                    }
                }

                success();
            }
        });
    }

    var calcPlayer = function (success) {
        $.ajax("Game/GetPlayer", {
            success: function (data) {
                var currentParty = [];
                for (var i = 0; i < data.currentPartyCharacters.length; i++) {
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

                if (data.isInCombat && !FuzzyOctoTribble.CombatControlCreator.inCombat()) {
                    FuzzyOctoTribble.CombatAccess.startCombat();
                }

                if (success) {
                    success();
                }
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
        periodicCheck();
        calcMap();
    }

    that.updateMap = function () {
        clearTimeout(playerTimer);
        calcMap();
        calcPlayer();
    }

    return that;
})();