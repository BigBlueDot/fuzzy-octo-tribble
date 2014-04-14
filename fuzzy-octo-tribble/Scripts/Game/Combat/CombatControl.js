FuzzyOctoTribble.CombatControlCreatorConstructor = function () {
    var that = {};

    var currentCommands;
    var createCommandSelectionScreen = function (commands) {
        var items = [];
        for (var i = 0; i < commands.length; i++) {
            items.push({
                text: commands[i].name,
                selected: function () {

                }
            });
        }
        var spec = {
            items: items,
            closeOnMenu: true,
            header: "Choose:"
        }
        var my = {};

        return FuzzyOctoTribble.Menu(spec, my);
    }

    that.createCommandSelectionScreen = createCommandSelectionScreen;

    that.createCharacterDetailScreen = function($detailScreen) {
        var that = {};

        $('.game-window').append($detailScreen);

        that.cancel = function () {
            $detailScreen.remove();
            that.onComplete();
        }

        that.menu = function () {
            that.cancel();
        }
        return that;
    }

    that.createBaseScreen = function (allies, enemies, commands) {
        var that = {};
        var $initialScreen = FuzzyOctoTribble.CombatScreenCreator.loadInitialScreen({
            allies: allies,
            enemies: enemies
        });

        $('.game-window').append($initialScreen);

        that.pressLeft = function () {

        }

        that.pressUp = function () {

        }

        that.pressRight = function () {

        }

        that.pressDown = function () {

        }

        that.releaseLeft = function () {

        }

        that.releaseUp = function () {

        }

        that.releaseDown = function () {

        }

        that.releaseRight = function () {

        }

        that.confirm = function () {

        }

        that.cancel = function () {

        }

        that.menu = function () {
            FuzzyOctoTribble.KeyControl.addController(createCommandSelectionScreen(commands));
        }

        return that;
    }

    that.create = function (spec, my) {
        FuzzyOctoTribble.KeyControl.addController(that.createBaseScreen(spec.characterDisplays, spec.npcDisplays, spec.commands));
        FuzzyOctoTribble.KeyControl.addController(that.createCommandSelectionScreen(spec.commands));
    }

    return that;
}