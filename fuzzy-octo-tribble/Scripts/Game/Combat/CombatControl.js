FuzzyOctoTribble.CombatControlCreatorConstructor = function () {
    var that = {};

    var createCommand = function (currentCommand) {
        var item = {
            text: currentCommand.name,
            selected: function () {
                var command = {};
                command.commandName = currentCommand.name;
                command.hasSubCommand = false;
                command.subCommand = {};
                command.targets = [];
                $.ajax({
                    type: 'POST',
                    url: 'Game/executeCommand',
                    cache: false,
                    data: JSON.stringify(command),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        fullData = data;
                        $.ajax("Game/getCommands", {
                            success: function (commandData) {
                                fullData.commands = commandData;
                                that.create(fullData);
                            }
                        });
                    }
                });

            }
        }
        return item;
    }

    var createCommandSelectionScreen = function (commands, currentCharacter) {
        var items = [];
        for (var i = 0; i < commands.length; i++) {
            var currentCommand = commands[i];
            items.push(createCommand(currentCommand));
        }
        var spec = {
            items: items,
            closeOnMenu: true,
            header: currentCharacter + ":",
            isCombat: true,
            additionalClasses: 'combat-screen'
        }
        var my = {};

        return FuzzyOctoTribble.Menu(spec, my);
    }

    var processEffects = function (effects) {
        if (effects.length === 0) {
            return;
        }
        var currentEffect = effects.shift();
        switch (currentEffect.type) {
            case 0: //Simple Message effects
                var messageSpec = {};
                messageSpec.dialogContent = currentEffect.message;
                messageSpec.onComplete = function () {
                    if (effects.length !== 0) {
                        processEffects(effects);
                    }
                }

                FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox(messageSpec, {}));
                break;
            case 1: //Deal damage to target
                var targetUniq = currentEffect.targetUniq;
                var damage = currentEffect.value;
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('-' + damage, targetUniq, '#FF0000', 24);
                processEffects(effects);
                break;
            case 2: //Heal the target
                var targetUniq = currentEffect.targetUniq;
                var heal = currentEffect.value;
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('+' + heal, targetUniq, '#00FF00', 24);
                processEffects(effects);
                break;
            case 3: //Destroy the character
                var targetUniq = currentEffect.targetUniq;
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('DEFEATED', targetUniq, '#FF0000', 30);
                processEffects(effects);
                break;
            case 4: //Combat has ended
                FuzzyOctoTribble.KeyControl.removeCombat();
                break;
            case 5: //Game over
                FuzzyOctoTribble.CombatScreenCreator.gameOverAnimation();
                processEffects(effects);
                break;

        }
    }

    that.createCommandSelectionScreen = createCommandSelectionScreen;

    that.createCharacterDetailScreen = function($detailScreen) {
        var that = {};

        $('.game-window').append($detailScreen);

        that.isCombat = true;
        that.isDetailWindow = true;

        that.close = function () {
            that.cancel();
        }

        that.cancel = function () {
            $detailScreen.remove();
            if (that.onComplete) {
                that.onComplete();
            }
        }

        that.menu = function () {
            that.cancel();
        }
        return that;
    }

    that.createBaseScreen = function (allies, enemies, commands, currentCharacter) {
        var that = {};

        $('.combat-screen').remove();
        var $initialScreen = FuzzyOctoTribble.CombatScreenCreator.loadInitialScreen({
            allies: allies,
            enemies: enemies
        });

        $('.game-window').append($initialScreen);

        that.isCombat = true;

        that.close = function () {
            $initialScreen.remove();
            if (that.onComplete) {
                that.onComplete();
            }
        }

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