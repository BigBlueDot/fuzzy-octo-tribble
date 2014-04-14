FuzzyOctoTribble.CombatControlCreatorConstructor = function () {
    var that = {};

    var createCommandSelectionScreen = function (commands, currentCharacter) {
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
            header: currentCharacter + ":"
        }
        var my = {};

        return FuzzyOctoTribble.Menu(spec, my);
    }

    var processEffects = function (effects) {
        var currentEffect = effects.shift();
        if (currentEffect.type === 0) { //Simple Message effects
            var messageSpec = {};
            messageSpec.dialogContent = currentEffect.message;
            messageSpec.onComplete = function () {
                if (effects.length !== 0) {
                    processEffects(effects);
                }
            }

            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox(messageSpec, {}));
        }
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

    that.createBaseScreen = function (allies, enemies, commands, currentCharacter) {
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
            FuzzyOctoTribble.KeyControl.addController(createCommandSelectionScreen(commands, currentCharacter));
        }

        return that;
    }

    that.create = function (spec, my) {
        FuzzyOctoTribble.KeyControl.addController(that.createBaseScreen(spec.characterDisplays, spec.npcDisplays, spec.commands, spec.currentCharacter));
        FuzzyOctoTribble.KeyControl.addController(that.createCommandSelectionScreen(spec.commands, spec.currentCharacter));
        if (spec.effects.length !== 0) {
            processEffects(spec.effects);
        }
    }

    return that;
}