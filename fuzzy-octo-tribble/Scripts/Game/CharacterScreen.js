FuzzyOctoTribble.CharacterScreen = (function () {
    var that = {};

    var $characterScreen = $(document.createElement('div'));
    $characterScreen.addClass('character-screen');
    $characterScreen.hide();
    var characterItems = [];
    var currentSelection = 0;

    var createCharacterItem = function (name, lvl, currentClass, classLvl, HP, MP) {
        var $characterItem = $(document.createElement('div'));
        $characterItem.addClass('character-screen-item text-font');
        var $characterLeft = $(document.createElement('span'));
        $characterLeft.text('Level ' + lvl + ' ' + currentClass + " Class Level: " + classLvl + " HP: " + HP + " MP: " + MP);
        var $characterRight = $(document.createElement('span'));
        $characterRight.text(name);
        $characterRight.css('float', 'right');

        $characterItem.append($characterLeft);
        $characterItem.append($characterRight);
        $characterScreen.append($characterItem);
        characterItems.push($characterItem);
    }

    var scrollDownOne = function () {
        for (var i = 0; i < characterItems.length; i++) {
            if (characterItems[i].is(":visible")) {
                characterItems[i].hide();
                break;
            }
        }
    }

    var scrollUpOne = function () {
        if (characterItems[0].is(':visible')) {
            return;
        }
        for (var i = 0; i < characterItems.length; i++) {
            if (characterItems[i].is(":visible")) {
                characterItems[i - 1].show();
                break;
            }
        }
    }

    $(document).ready(function () {
        $('.game-window').append($characterScreen);
    });

    that.setCharacters = function (characters) {
        $characterScreen.empty();
        characterItems = [];
        for (var i = 0; i < characters.length; i++) {
            var character = characters[i];
            var currentCharacterClass;
            for(var j=0; j<character.characterClasses.length; j++) {
                if(character.characterClasses[i].className === character.currentClass) {
                    currentCharacterClass = character.characterClasses[i];
                }
            }
            createCharacterItem(character.name, character.lvl, currentCharacterClass.className, currentCharacterClass.lvl, character.stats.maxHP, character.stats.maxMP);
        }
        characterItems[currentSelection].addClass('character-screen-item-selected');
    }

    that.showCharacterScreen = function () {
        $characterScreen.show();
        currentSelection = 0;
        $('.character-screen-item-selected').removeClass('character-screen-item-selected');
        characterItems[currentSelection].addClass('character-screen-item-selected');
    }

    that.hideCharacterScreen = function () {
        $characterScreen.hide();
    }

    that.scrollUp = function () {
        if (currentSelection == 0) {
            return;
        }

        $('.character-screen-item-selected').removeClass('character-screen-item-selected');
        currentSelection--;
        characterItems[currentSelection].addClass('character-screen-item-selected');
        if (!characterItems[currentSelection].is(":visible")) {
            scrollUpOne();
        }
    }

    that.scrollDown = function () {
        if (currentSelection == characterItems.length - 1) {
            return;
        }

        $('.character-screen-item-selected').removeClass('character-screen-item-selected');
        currentSelection++;
        characterItems[currentSelection].addClass('character-screen-item-selected');
        if (characterItems[currentSelection].position().top >= $characterScreen.height()) {
            scrollDownOne();
        }
    }

    return that;
})();