FuzzyOctoTribble.CombatControlCreatorConstructor = function () {
    var that = {};

    var selectingTarget = false;
    var onTargetSelected;
    var $currentDefaultScreen, $currentCommandScreen;
    var gameOver = false;
    var inCombat = false;
    var commandScreens = [];
    var placeholderKeyControl = {
        isCombat: true, close: function () {
            if (this.onComplete) {
                this.onComplete();
            }
        }};

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
                            $currentCommandScreen.close();
                            for (var i = 0; i < commandScreens.length; i++) {
                                commandScreens[i].close();
                            }
                            commandScreens = [];
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
                    $currentDefaultScreen.isCombat = true;
                    FuzzyOctoTribble.KeyControl.addController($currentDefaultScreen);
                    $currentDefaultScreen.onCurrentControl();
                    onTargetSelected = function (characterUniq) {
                        sendingCommand.targets.push(characterUniq);
                        $currentDefaultScreen.onComplete();
                        $currentDefaultScreen.clearCurrentControl();
                        onComplete();
                    }
                }
                else if (currentCommand.hasChildCommands) {
                    commandScreens.push($currentCommandScreen);
                    $currentCommandScreen = createCommandSelectionScreen(currentCommand.childCommands, currentCharacter, sendingCommand.subCommand, onComplete);
                    $currentCommandScreen.show();
                    FuzzyOctoTribble.KeyControl.addController($currentCommandScreen);
                }
                else {
                    onComplete();
                }
            },
            isDisabled: currentCommand.isDisabled
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
            additionalClasses: 'combat-screen',
            delayShow: true
        }
        var my = {};

        return FuzzyOctoTribble.Menu(spec, my);
    }

    var processEffects = function (effects) {
        if (effects.length === 0) {
            return;
        }
        FuzzyOctoTribble.KeyControl.addController(placeholderKeyControl);
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
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('-' + damage, targetUniq, '#FF0000', 32);
                processEffects(effects);
                break;
            case 2: //Heal the target
                var targetUniq = currentEffect.targetUniq;
                var heal = currentEffect.value;
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('+' + heal, targetUniq, '#00FF00', 32);
                processEffects(effects);
                break;
            case 3: //Destroy the character
                var targetUniq = currentEffect.targetUniq;
                FuzzyOctoTribble.CombatScreenCreator.numberAnimation('DEFEATED', targetUniq, '#FF0000', 44);
                FuzzyOctoTribble.CombatScreenCreator.defeat(targetUniq);
                processEffects(effects);
                break;
            case 4: //Combat has ended
                placeholderKeyControl.onComplete();
                FuzzyOctoTribble.KeyControl.removeCombat();
                $currentDefaultScreen = false;
                $currentCommandScreen = false;
                if (gameOver) {
                    FuzzyOctoTribble.MaintainState.updateMap();
                }
                inCombat = false;
                break;
            case 5: //Game over
                FuzzyOctoTribble.CombatScreenCreator.gameOverAnimation();
                gameOver = true;
                processEffects(effects);
                inCombat = false;
                break;
            case 6: //Show Command Screen
                $currentCommandScreen.show();
                placeholderKeyControl.onComplete();
                FuzzyOctoTribble.KeyControl.addController($currentCommandScreen);
                processEffects(effects);
                break;
            case 7: //End turn
                FuzzyOctoTribble.CombatAccess.continueCombat();
                break;
            case 8: //Enemy attack animation
                FuzzyOctoTribble.CombatScreenCreator.attackAnimation(currentEffect.targetUniq, function () {
                    processEffects(effects);
                });
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

        var pcCurrentlySelected = false;
        var currentSelection = 0;
        var commands = commands;
        var currentCharacter = currentCharacter;

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
            $initialScreen.find('.character-display-screen').addClass('character-display-screen-selectable');
            selectCurrent();
        }

        that.clearCurrentControl = function () {
            $initialScreen.find('.character-display-screen').removeClass('character-display-screen-selectable');
        }

        var deselectCurrent = function () {
            if (pcCurrentlySelected) {
                allies[currentSelection].onDeselect();
            }
            else {
                enemies[currentSelection].onDeselect();
            }
        }

        var selectCurrent = function () {
            if (pcCurrentlySelected) {
                allies[currentSelection].onSelect();
            }
            else {
                enemies[currentSelection].onSelect();
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
            if (currentSelection != 0) {
                deselectCurrent();
                currentSelection--;
                selectCurrent();
            }
        }

        that.releaseUp = function () {
            deselectCurrent();
            pcCurrentlySelected = false;
            if (currentSelection >= enemies.length) {
                currentSelection = enemies.length - 1;
            }
            selectCurrent();
        }

        that.releaseDown = function () {
            deselectCurrent();
            pcCurrentlySelected = true;
            if (currentSelection >= allies.length) {
                currentSelection = allies.length - 1;
            }
            selectCurrent();
        }

        that.releaseRight = function () {
            if ((pcCurrentlySelected && currentSelection < allies.length - 1) ||
                (!pcCurrentlySelected && currentSelection < enemies.length - 1)) {
                deselectCurrent();
                currentSelection++;
                selectCurrent();
            }
        }

        that.confirm = function () {
            if (pcCurrentlySelected) {
                allies[currentSelection].click();
            }
            else {
                enemies[currentSelection].click();
            }
        }

        that.cancel = function () {
            if (selectingTarget) {
                if (this.onComplete) {
                    this.onComplete();
                }
                selectingTarget = false;
            }
            else {
                that.menu();
            }
        }

        that.menu = function () {
            $currentCommandScreen = createCommandSelectionScreen(commands, currentCharacter);
            $currentCommandScreen.show();
            FuzzyOctoTribble.KeyControl.addController($currentCommandScreen);
        }

        that.updateInformation = function (allies, enemies, commands, currentCharacter) {
            commands = commands;
            currentCharacter = currentCharacter;
            $initialScreen.update(allies, enemies);
        }

        return that;
    }

    that.create = function (spec, my) {
        if ($currentDefaultScreen) {
            $currentDefaultScreen.updateInformation(spec.characterDisplays, spec.npcDisplays, spec.commands, spec.currentCharacter);
        }
        else {
            $currentDefaultScreen = that.createBaseScreen(spec.characterDisplays, spec.npcDisplays, spec.commands, spec.currentCharacter);
            FuzzyOctoTribble.KeyControl.addController($currentDefaultScreen);
        }

        $currentCommandScreen = that.createCommandSelectionScreen(spec.commands, spec.currentCharacter);
        if (spec.effects.length !== 0) {
            processEffects(spec.effects);
        }
        inCombat = true;
    }

    that.inCombat = function () {
        return inCombat;
    }

    return that;
}