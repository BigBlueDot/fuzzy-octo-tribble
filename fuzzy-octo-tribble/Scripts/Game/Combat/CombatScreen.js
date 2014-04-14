FuzzyOctoTribble.CombatScreenCreator = (function () {
    var that = {};
    var allies, enemies;
    var characterWindows = {};

    var getCharacterWindow = function (character) {
        var $characterDisplay = $(document.createElement('div'));
        $characterDisplay.addClass('character-display-screen text-font');
        $characterDisplay.append($(document.createElement('div')).text(character.name));
        $characterDisplay.append($(document.createElement('div')).text("HP: " + character.hp + " / " + character.maxHP));
        $characterDisplay.append($(document.createElement('div')).text("MP: " + character.mp + " / " + character.maxMP));
        $characterDisplay.on('click', function () {
            var $detailScreen = that.getDetailedScreen(character);
            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CombatControlCreator.createCharacterDetailScreen($detailScreen));
        });
        characterWindows[character.uniq] = $characterDisplay;
        return $characterDisplay;
    }

    that.damageAnimation = function (value, uniq) {
        var $damageWindow = $(document.createElement('div'));
        $damageWindow.addClass('text-font');
        $damageWindow.text("-" + value);
        $damageWindow.css('color', '#FF0000');
        $damageWindow.css('position', 'absolute');
        $damageWindow.css('top', (characterWindows[uniq].position().top + 10) + 'px');
        $damageWindow.css('left', (characterWindows[uniq].position().left + 140) + 'px');
        $('.game-window').append($damageWindow);
        $damageWindow.animate({ height: 'toggle', opacity: 'toggle' }, 2000, 'swing', function () {
            $damageWindow.remove();
        });
    }

    that.loadInitialScreen = function (spec, my) {
        allies = spec.allies;
        enemies = spec.enemies;
        var $content = $(document.createElement('div'));
        $content.addClass('combat-initial-screen');

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
        $detailScreen.addClass('popup-screen text-font character-detail-screen');

        var $nameLvl = $(document.createElement('div'));
        $nameLvl.text(character.name + " Level " + character.lvl);

        var $class = $(document.createElement('div'));
        $class.text("Character Type:  " + character.currentClass);

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