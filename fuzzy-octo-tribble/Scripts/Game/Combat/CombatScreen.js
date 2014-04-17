FuzzyOctoTribble.CombatScreenCreator = (function () {
    var that = {};
    var allies, enemies;
    var characterWindows = {};

    var getCharacterWindow = function (character) {
        var $characterDisplay = $(document.createElement('div'));
        $characterDisplay.addClass('character-display-screen text-font');
        $characterDisplay.append($(document.createElement('div')).text(character.name));
        $characterDisplay.append($(document.createElement('div')).text('Attack Order: ' + character.turnOrder));
        $characterDisplay.append($(document.createElement('div')).text("HP: " + character.hp + " / " + character.maxHP));
        $characterDisplay.append($(document.createElement('div')).text("MP: " + character.mp + " / " + character.maxMP));
        for (var i = 0; i < character.statuses.length; i++) {
            var currentStatus = character.statuses[i];
            $characterDisplay.append($(document.createElement('div')).text(currentStatus.value));
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
        }
        if (character.hp <= 0) {
            $characterDisplay.css('border-color', '#ff0000');
            $characterDisplay.css('background-color', '#000000');
            $characterDisplay.css('color', '#ffffff');
        }
        $characterDisplay.on('click', function () {
            if (character.selected) {
                character.selected(character);
            }
        });
        characterWindows[character.uniq] = $characterDisplay;
        return $characterDisplay;
    }

    that.numberAnimation = function (value, uniq, color, size) {
        var $damageWindow = $(document.createElement('div'));
        $damageWindow.addClass('text-font');
        $damageWindow.text(value);
        $damageWindow.css('color', color);
        $damageWindow.css('font-size', size);
        $damageWindow.css('position', 'absolute');
        $damageWindow.css('top', (characterWindows[uniq].position().top + 10) + 'px');
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
        var $content = $(document.createElement('div'));
        $content.addClass('combat-initial-screen combat-screen');

        for (var i = 0; i < allies.length; i++) {
            var $characterDisplay = getCharacterWindow(allies[i]);
            $characterDisplay.css('left', (20 + (230 * i)).toString() + "px");
            $content.append($characterDisplay);
        }

        for (var i = 0; i < enemies.length; i++) {
            var $characterDisplay = getCharacterWindow(enemies[i]);
            $characterDisplay.addClass('enemy');
            $characterDisplay.css('left', (20 + (230 * i)).toString() + "px");
            $content.append($characterDisplay);
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

        $detailScreen.append($nameLvl);
        $detailScreen.append($class);
        $detailScreen.append($HP);
        $detailScreen.append($MP);
        return $detailScreen
    }

    return that;
})();