FuzzyOctoTribble.CombatAccess = (function () {
    var that = {};

    that.getState = function (success, isContinue) {
        var fullData = {};
        var address = 'Game/getStatus';
        if (isContinue) {
            address = 'Game/nextTurn';
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