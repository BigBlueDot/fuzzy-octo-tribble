﻿FuzzyOctoTribble.MaintainState = (function () {
    var that = {};

    var maintainTimer;
    var paused = false;

    var periodicCheck = function () {
        if (paused) {
            return;
        }
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
                for (var i = data.length - 1; i >= 0; i--) {
                    switch (data[i].type) {
                        case 0: //Display message
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox({ dialogContent: data[i].message }));
                            break;
                        case 1: //Redraw map
                            calcMap(true);
                            break;
                        case 2: //Run the current event
                            FuzzyOctoTribble.Movement.feignMove();
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

    var calcMap = function (doDraw) {
        $.ajax("Game/GetMap", {
            success: function (data) {
                FuzzyOctoTribble.Camera.setMap(data, doDraw);
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
        calcMap();
        calcPlayer();
    }

    that.checkNow = function () {
        window.clearTimeout(maintainTimer);
        periodicCheck();
    }

    that.pause = function () {
        paused = true;
        window.clearTimeout(maintainTimer);
    }

    that.restart = function () {
        paused = false;
        periodicCheck();
    }

    return that;
})();