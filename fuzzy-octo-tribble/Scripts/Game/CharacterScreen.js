FuzzyOctoTribble.CharacterScreen = (function () {
    var that = {};

    var $characterScreen = $(document.createElement('div'));
    $characterScreen.addClass('character-screen');
    $characterScreen.hide();
    var characterItems = [];
    var characters = [];
    var currentSelection = 0;
    var shouldHide = true;

    var createCharacterItem = function (name, lvl, currentClass, classLvl, HP, MP, STR, VIT, INT, WIS, AGI) {
        var character = {
            name: name,
            lvl: lvl,
            currentClass: currentClass,
            classLvl: classLvl,
            HP: HP,
            MP: MP,
            STR: STR,
            VIT: VIT,
            INT: INT,
            WIS: WIS,
            AGI: AGI
        };

        var $characterItem = $(document.createElement('div'));
        $characterItem.addClass('character-screen-item text-font');
        var $characterLeft = $(document.createElement('span'));
        $characterLeft.text('Level ' + lvl + ' ' + currentClass + " Class Level: " + classLvl + " HP: " + HP + " MP: " + MP);
        var $characterRight = $(document.createElement('span'));
        $characterRight.text(name);
        $characterRight.css('float', 'right');

        $characterItem.click(function () {
            var $screen = createCharacterDetailScreen(character.name, character.lvl, character.currentClass, character.classLvl, character.HP, character.MP, character.STR, character.VIT, character.INT, character.WIS, character.AGI);
            $('.game-window').append($screen);
            shouldHide = false;
            $screen.show();
        });

        $characterItem.append($characterLeft);
        $characterItem.append($characterRight);
        $characterScreen.append($characterItem);
        characterItems.push($characterItem);
    }

    var createCharacterDetailScreen = function (name, lvl, currentClass, classLvl, HP, MP, STR, VIT, INT, WIS, AGI) {
        var $detailScreen = $(document.createElement('div'));
        $detailScreen.addClass('character-screen text-font character-detail-screen');
        
        var $nameLvl = $(document.createElement('div'));
        $nameLvl.text(name + " Level " + lvl);

        var $class = $(document.createElement('div'));
        $class.text(currentClass + ' Level ' + classLvl);

        var $HPMP = $(document.createElement('div'));
        $HPMP.text('HP: ' + HP + ' MP: ' + MP);

        var $STRVIT = $(document.createElement('div'));
        $STRVIT.text('STR: ' + STR + ' VIT: ' + VIT);
        
        var $INTWIS = $(document.createElement('div'));
        $INTWIS.text('INT: ' + INT + ' WIS: ' + WIS);

        var $AGI = $(document.createElement('div'));
        $AGI.text("AGI: " + AGI);

        $detailScreen.append($nameLvl);
        $detailScreen.append($class);
        $detailScreen.append($HPMP);
        $detailScreen.append($STRVIT);
        $detailScreen.append($INTWIS);
        $detailScreen.append($AGI);

        return $detailScreen;
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
            createCharacterItem(character.name, character.lvl, currentCharacterClass.className, currentCharacterClass.lvl, character.stats.maxHP, character.stats.maxMP, character.stats.strength, character.stats.vitality, character.stats.intellect, character.stats.wisdom, character.stats.agility);
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

    that.selectCurrent = function () {
        characterItems[currentSelection].click();
    }

    that.canHandleCancel = function () {
        return !shouldHide;
    }

    that.cancel = function () {
        if (!shouldHide) {
            $('.character-detail-screen').hide();
            shouldHide = true;
        }
    }

    return that;
})();