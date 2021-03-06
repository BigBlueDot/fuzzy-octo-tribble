﻿FuzzyOctoTribble.CombatScreenCreator = (function () {
    var that = {};
    var allies, enemies;
    var characterWindows = {};

    var getCharacterWindow = function (character, location) {
        var $characterDisplay = $(document.createElement('div'));
        var $name = $(document.createElement('div')).text(character.name);
        var $type = $(document.createElement('div')).text(character.type);
        var $turnOrder = $(document.createElement('div')).text('Turn Order:' + character.turnOrder);
        var $HP = $(document.createElement('div')).text("HP: " + character.hp + "/" + character.maxHP);
        var $MP = $(document.createElement('div')).text("MP: " + character.mp + "/" + character.maxMP);
        var $statuses = $(document.createElement('div'));
        $characterDisplay.addClass('character-display-screen text-font ' + location);
        $characterDisplay.append($name);
        $characterDisplay.append($type);
        $characterDisplay.append($turnOrder);
        $characterDisplay.append($HP);
        $characterDisplay.append($MP);
        $characterDisplay.append($statuses);
        var $imageDisplay = $(document.createElement('div')).addClass(character.type + ' character-display-screen image');
        FuzzyOctoTribble.CombatAnimation.addAnimation($imageDisplay);
        $characterDisplay.append($imageDisplay);

        $characterDisplay.update = function (character) {
            $statuses.empty();
            $name.text(character.name);
            $type.text(character.type);
            $turnOrder.text('Turn Order:' + character.turnOrder);
            $HP.text("HP: " + character.hp + "/" + character.maxHP);
            $MP.text("MP: " + character.mp + "/" + character.maxMP);
            if (character.hp < character.maxHP / 2) {
                $characterDisplay.addClass('Wounded');
            } else {
                $characterDisplay.removeClass('Wounded');
            }

            for (var i = 0; i < character.statuses.length; i++) {
                var currentStatus = character.statuses[i];
                $statuses.append($(document.createElement('div')).text(currentStatus.value));
            }
            switch (character.turnOrder) {
                case 1:
                    $characterDisplay.css('border-color', '#ffd700');
                    break;
                case 2:
                    $characterDisplay.css('border-color', '#C4AEAD ');
                    break;
                case 3:
                    $characterDisplay.css('border-color', '#cd7f32');
                    break;
                default:
                    $characterDisplay.css('border-color', '#000000');
            }
            if (character.hp <= 0) {
                $characterDisplay.css('border-color', '#ff0000');
                $characterDisplay.css('background-color', '#000000');
                $characterDisplay.css('color', '#ffffff');
                $characterDisplay.addClass('Defeated');
            }
        }

        $characterDisplay.on('click', function () {
            if (character.selected) {
                character.selected(character);
            }
        });
        characterWindows[character.uniq] = $characterDisplay;

        character.onSelect = function () {
            $characterDisplay.addClass('character-item-selected');
        }

        character.onDeselect = function () {
            $characterDisplay.removeClass('character-item-selected');
        }

        character.click = function () {
            $characterDisplay.click();
        }

        $characterDisplay.update(character);

        return $characterDisplay;
    }

    that.attackAnimation = function (uniq, success) {
        $characterDisplay = characterWindows[uniq];
        var count = 4;

        var processAnimation = function () {
            count--;
            if ($characterDisplay.hasClass('Attack')) {
                $characterDisplay.removeClass('Attack');
                $characterDisplay.addClass('PreAttack');
            }
            else {
                $characterDisplay.addClass('Attack');
                $characterDisplay.removeClass('PreAttack');
            }
        }

        var setAnimationTimer = function() {
            processAnimation();
            if (count == 0) {
                $characterDisplay.removeClass('Attack');
                $characterDisplay.removeClass('PreAttack');
                success();
            }
            else {
                setTimeout(setAnimationTimer, 200);
            }
        }

        setAnimationTimer();
    }

    that.defeat = function (uniq) {
        characterWindows[uniq].addClass('Defeated');
    }

    that.numberAnimation = function (value, uniq, color, size) {
        var $damageWindow = $(document.createElement('div'));
        $damageWindow.addClass('text-font damage-display');
        $damageWindow.text(value);
        $damageWindow.css('color', color);
        $damageWindow.css('font-size', size);
        $damageWindow.css('position', 'absolute');
        $damageWindow.css('top', (characterWindows[uniq].position().top + characterWindows[uniq].height() - 25) + 'px');
        $damageWindow.css('left', (characterWindows[uniq].position().left + 140) + 'px');
        $('.game-window').append($damageWindow);
        $damageWindow.animate({ height: 'toggle', opacity: 'toggle' }, 2000, 'swing', function () {
            $damageWindow.remove();
        });
    }

    that.gameOverAnimation = function () {
        var $gameOverWindow = $(document.createElement('div'));
        $gameOverWindow.addClass('text-font');
        $gameOverWindow.text("Game Over");
        $gameOverWindow.css('color', "#FF0000");
        $gameOverWindow.css('text-align', 'center');
        $gameOverWindow.css('font-size', "60px");
        $gameOverWindow.css('position', 'absolute');
        $gameOverWindow.css('top', (($(window).height() / 2) - 15) + 'px');
        $gameOverWindow.css('left', '10px');
        $gameOverWindow.css('right', '10px');
        $('.game-window').append($gameOverWindow);
        $gameOverWindow.animate({ height: 'toggle', opacity: 'toggle' }, 5000, 'swing', function () {
            $gameOverWindow.remove();
        });
    }

    that.loadInitialScreen = function (spec, my) {
        allies = spec.allies;
        enemies = spec.enemies;
        var location = spec.location;
        var $content = $(document.createElement('div'));
        $content.addClass('combat-initial-screen combat-screen');

        for (var i = 0; i < allies.length; i++) {
            var $characterDisplay = getCharacterWindow(allies[i], location);
            $characterDisplay.css('left', (20 + (230 * i)).toString() + "px");
            $content.append($characterDisplay);
        }

        for (var i = 0; i < enemies.length; i++) {
            var $characterDisplay = getCharacterWindow(enemies[i], location);
            $characterDisplay.addClass('enemy');
            $characterDisplay.css('left', (20 + (230 * i)).toString() + "px");
            $content.append($characterDisplay);
        }

        $content.update = function (alliesP, enemiesP) {
            allies = alliesP;
            enemies = enemiesP;

            for (var i = 0; i < allies.length; i++) {
                characterWindows[allies[i].uniq].update(allies[i]);
            }
            for (var i = 0; i < enemies.length; i++) {
                characterWindows[enemies[i].uniq].update(enemies[i]);
            }
        }

        return $content;
    }
    
    that.getDetailedScreen = function (character) {
        var $detailScreen = $(document.createElement('div'));
        $detailScreen.addClass('popup-screen text-font character-detail-screen combat-screen');

        var $nameLvl = $(document.createElement('div'));
        $nameLvl.text(character.name + " Level " + character.level);

        var $class = $(document.createElement('div'));
        $class.text("Character Type:  " + character.type);

        var $HP = $(document.createElement('div'));
        $HP.text('HP: ' + character.hp + ' / ' + character.maxHP);

        var $MP = $(document.createElement('div'));
        $MP.text('MP: ' + character.mp + ' / ' + character.maxMP);

        if (character.showSpecificStats) {
            var $STRVIT = $(document.createElement('div'));
            $STRVIT.text('STR: ' + character.strength + ' VIT: ' + character.vitality);

            var $INTWIS = $(document.createElement('div'));
            $INTWIS.text('INT: ' + character.intellect + ' WIS: ' + character.wisdom);

            var $AGI = $(document.createElement('div'));
            $AGI.text("AGI: " + character.agility);

            var $description = $(document.createElement('div'));
            $description.text(character.description);
        }

        $detailScreen.append($nameLvl);
        $detailScreen.append($class);
        $detailScreen.append($HP);
        $detailScreen.append($MP);
        
        if (character.showSpecificStats) {
            $detailScreen.append($STRVIT);
            $detailScreen.append($INTWIS);
            $detailScreen.append($AGI);
            $detailScreen.append($description);
        }
        return $detailScreen
    }

    return that;
})();