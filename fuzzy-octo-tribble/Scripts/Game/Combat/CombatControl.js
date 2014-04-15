FuzzyOctoTribble.CombatControlCreatorConstructor = function () {
    var that = {};

    var selectingTarget = false;
    var onTargetSelected;
    var $currentDefaultScreen;

    var createCommand = function (currentCommand, currentCharacter, sendingCommand, onComplete) {
        var executeFinalCommand = function (cmd) {
            selectingTarget = false;
            $.ajax({
                type: 'POST',
                url: 'Game/executeCommand',
                cache: false,
                data: JSON.stringify(cmd),
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

        if (!sendingCommand) {
            var sendingCommand = {};
        }
        if (!onComplete) {
            var onComplete = function () {
                executeFinalCommand(sendingCommand);
            }
        }

        var commandText;
        commandText = currentCommand.name;
        if (currentCommand.mpNeeded) {
            commandText = currentCommand.mpCost + ' MP - ' + currentCommand.name;
        }
        if (currentCommand.limitedUsage) {
            commandText += " " + currentCommand.uses + "/" + currentCommand.totalUses;
        }

        var item = {
            text: commandText,
            selected: function () {
                sendingCommand.commandName = currentCommand.name;
                sendingCommand.hasSubCommand = false;
                sendingCommand.subCommand = {};
                sendingCommand.targets = [];

                if (currentCommand.hasTarget) {
                    selectingTarget = true;
                    $currentDefaultScreen.onCurrentControl();
                    onTargetSelected = function (characterUniq) {
                        sendingCommand.targets.push(characterUniq);
                        $currentDefaultScreen.clearCurrentControl();
                        onComplete();
                    }
                }
                else if (currentCommand.hasChildCommands) {
                    FuzzyOctoTribble.KeyControl.addController(createCommandSelectionScreen(currentCommand.childCommands, currentCharacter, sendingCommand.subCommand, onComplete));
                }
                else {
                    onComplete();
                }
            }
        }
        return item;
    }

    var createCommandSelectionScreen = function (commands, currentCharacter, sendingCommand, onComplete) {
        var items = [];
        for (var i = 0; i < commands.length; i++) {
            var currentCommand = commands[i];
            items.push(createCommand(currentCommand, currentCharacter, sendingCommand, onComplete));
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

        var onCharacterSelect = function (character) {
            if (selectingTarget) {
                onTargetSelected(character.uniq);
            }
            else if (that.hasCurrentControl) {
                var $detailScreen = FuzzyOctoTribble.CombatScreenCreator.getDetailedScreen(character);
                FuzzyOctoTribble.KeyControl.removeWindows('isDetailWindow');
                FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CombatControlCreator.createCharacterDetailScreen($detailScreen));
            }
        }

        for (var i = 0; i < allies.length; i++) {
            allies[i].selected = onCharacterSelect;
        }

        for (var i = 0; i < enemies.length; i++) {
            enemies[i].selected = onCharacterSelect;
        }

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

        that.onCurrentControl = function () {
            $initialScreen.find('.character-display-screen').css('cursor', 'pointer');
        }

        that.clearCurrentControl = function () {
            $initialScreen.find('.character-display-screen').css('cursor', 'initial');
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
        $currentDefaultScreen = that.createBaseScreen(spec.characterDisplays, spec.npcDisplays, spec.commands, spec.currentCharacter);
        FuzzyOctoTribble.KeyControl.addController($currentDefaultScreen);
        FuzzyOctoTribble.KeyControl.addController(that.createCommandSelectionScreen(spec.commands, spec.currentCharacter));
        if (spec.effects.length !== 0) {
            processEffects(spec.effects);
        }
    }

    return that;
}