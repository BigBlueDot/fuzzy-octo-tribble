FuzzyOctoTribble.CombatAccess = (function () {
    var that = {};

    that.getState = function (success) {
        var fullData = {};
        $.ajax("Game/getStatus", {
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

    return that;
})();