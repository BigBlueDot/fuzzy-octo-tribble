FuzzyOctoTribble.CharacterScreenCreator = (function () {
    var that = {};
    var characterItems = [];
    var currentPartyItems = [];

    that.setCharacters = function (characters, currentParty) {
        characterItems = [];
        for (var i = 0; i < characters.length; i++) {
            var character = characters[i];
            var currentCharacterClass;
            for (var j = 0; j < character.characterClasses.length; j++) {
                if (character.characterClasses[i].className === character.currentClass) {
                    currentCharacterClass = character.characterClasses[i];
                }
            }

            characterItems.push(createCharacterItem(character.name, character.lvl, currentCharacterClass.className, currentCharacterClass.lvl, character.stats.maxHP, character.stats.maxMP, character.stats.strength, character.stats.vitality, character.stats.intellect, character.stats.wisdom, character.stats.agility, character.xp, character.xpToLevel));
        }

        currentPartyItems = [];
        for (var i = 0; i < currentParty.length; i++) {
            var character = currentParty[i];
            var currentCharacterClass;
            for (var j = 0; j < character.characterClasses.length; j++) {
                if (character.characterClasses[i].className === character.currentClass) {
                    currentCharacterClass = character.characterClasses[i];
                }
            }

            currentPartyItems.push(createCharacterItem(character.name, character.lvl, currentCharacterClass.className, currentCharacterClass.lvl, character.stats.maxHP, character.stats.maxMP, character.stats.strength, character.stats.vitality, character.stats.intellect, character.stats.wisdom, character.stats.agility, character.xp, character.xpToLevel));
        }
    }

    var createCharacterItem = function (name, lvl, currentClass, classLvl, HP, MP, STR, VIT, INT, WIS, AGI, xp, xpToLevel) {
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
            AGI: AGI,
            xp: xp,
            xpToLevel: xpToLevel
        };

        var $characterItem = $(document.createElement('div'));
        $characterItem.addClass('popup-screen-item text-font');
        var $characterLeft = $(document.createElement('span'));
        $characterLeft.text('Level ' + lvl + ' ' + currentClass + " Class Level: " + classLvl + " HP: " + HP + " MP: " + MP);
        var $characterRight = $(document.createElement('span'));
        $characterRight.text(name);
        $characterRight.css('float', 'right');

        $characterItem.append($characterLeft);
        $characterItem.append($characterRight);

        return {
            content: $characterItem,
            select: function () {
                FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenControl(createCharacterDetailScreen(character.name, character.lvl, character.currentClass, character.classLvl, character.HP, character.MP, character.STR, character.VIT, character.INT, character.WIS, character.AGI, character.xp, character.xpToLevel), true, function () {
                    FuzzyOctoTribble.KeyControl.menu();
                }));
            },
            value: name
        };
    }

    var createCharacterDetailScreen = function (name, lvl, currentClass, classLvl, HP, MP, STR, VIT, INT, WIS, AGI, xp, xpToLevel) {
        var $detailScreen = $(document.createElement('div'));
        $detailScreen.addClass('popup-screen text-font character-detail-screen');

        var $nameLvl = $(document.createElement('div'));
        $nameLvl.text(name + " Level " + lvl);

        var $xp = $(document.createElement('div'));
        $xp.text(xp + "/" + xpToLevel + " XP");

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
        $detailScreen.append($xp);
        $detailScreen.append($class);
        $detailScreen.append($HPMP);
        $detailScreen.append($STRVIT);
        $detailScreen.append($INTWIS);
        $detailScreen.append($AGI);

        return $detailScreen;
    }

    that.getScreen = function () {
        return FuzzyOctoTribble.ScreenSelectControl({
            items: characterItems,
            closeOnMenu: true,
            onCloseMenu: function () {
                FuzzyOctoTribble.KeyControl.menu();
            }
        });
    }

    that.getPartySelectScreen = function (maxSelectCount, onSelectComplete) {
        return FuzzyOctoTribble.ScreenMultiSelectControl({
            items: characterItems,
            onSelectComplete: onSelectComplete,
            maxSelectCount: maxSelectCount
        });
    }

    that.getPartyCharacters = function () {
        return FuzzyOctoTribble.ScreenSelectControl({
            items: currentPartyItems,
            closeOnMenu: true,
            onCloseMenu: function () {
                FuzzyOctoTribble.KeyControl.menu();
            }
        });
    }

    return that;
})();