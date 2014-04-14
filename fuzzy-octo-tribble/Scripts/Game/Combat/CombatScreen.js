FuzzyOctoTribble.CombatScreenCreator = (function () {
    var that = {};
    var allies, enemies;

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
        return $characterDisplay;
    }

    that.loadInitialScreen = function (spec, my) {
        allies = spec.allies;
        enemies = spec.enemies;
        var $content = $(document.createElement('div'));

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

    that.loadCommand = function (commands) {
        var items = [];
        for (var i = 0; i < commands.length; i++) {
            items.push({
                text: commands[i],
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

        FuzzyOctoTribble.Menu(spec, my);
    }

    return that;
})();