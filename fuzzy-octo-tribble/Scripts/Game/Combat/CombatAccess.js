FuzzyOctoTribble.CombatAccess = (function () {
    var that = {};
    var combatStartedRecently = false; //This variable is to fix a weird timing error I couldn't track down

    that.getState = function (success, isContinue) {
        if (combatStartedRecently && !isContinue) {
            return; //Phantom start of combat
        }
        var fullData = {};
        var address = 'Game/getStatus';
        if (isContinue) {
            address = 'Game/nextTurn';
            combatStartedRecently = false;
        }
        else {
            combatStartedRecently = true;
        }
        $.ajax(address, {
            success: function (data) {
                fullData = data;
                $.ajax("Game/getCommands", {
                    success: function (commandData) {
                        fullData.commands = commandData;
                        success(fullData);
                    }
                });
            }
        })
    }

    that.startCombat = function () {
        that.getState(function (data) {
            FuzzyOctoTribble.CombatControlCreator.create(data);
        });
    }

    that.continueCombat = function () {
        that.getState(function (data) {
            FuzzyOctoTribble.CombatControlCreator.create(data);
        }, true);
    }

    return that;
})();