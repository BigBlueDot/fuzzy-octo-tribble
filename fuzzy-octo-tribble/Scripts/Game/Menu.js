FuzzyOctoTribble.Menu = (function () {
    var that = {};

    var currentMode = 'menu';

    var $menu = $(document.createElement('div'));
    $menu.addClass('menu-container');

    var $menuTitleItem = $(document.createElement('div'));
    $menuTitleItem.addClass('menu-item menu-title text-font');
    $menuTitleItem.text('Menu');

    var $menuCharactersItem = $(document.createElement('div'));
    $menuCharactersItem.addClass('menu-item text-font');
    $menuCharactersItem.text('Characters');
    $menuCharactersItem.on('click', function () { selectItem($menuCharactersItem); });

    var $menuPartiesItem = $(document.createElement('div'));
    $menuPartiesItem.addClass('menu-item text-font');
    $menuPartiesItem.text('Parties');
    $menuPartiesItem.on('click', function () { selectItem($menuPartiesItem); });

    var $menuQuestsItem = $(document.createElement('div'));
    $menuQuestsItem.addClass('menu-item text-font');
    $menuQuestsItem.text('Quests');
    $menuQuestsItem.on('click', function () { selectItem($menuQuestsItem); });

    var $menuConfigurationItem = $(document.createElement('div'));
    $menuConfigurationItem.addClass('menu-item text-font');
    $menuConfigurationItem.text('Configuration');
    $menuConfigurationItem.on('click', function () { selectItem($menuConfigurationItem); });
    
    $menu.append($menuTitleItem);
    $menu.append($menuCharactersItem);
    $menu.append($menuPartiesItem);
    $menu.append($menuQuestsItem);
    $menu.append($menuConfigurationItem);
    $menu.hide();

    var selectedMenuItem = 0;
    var menuItems = [];
    menuItems.push($menuCharactersItem);
    menuItems.push($menuPartiesItem);
    menuItems.push($menuQuestsItem);
    menuItems.push($menuConfigurationItem);

    var $goldDisplay = $(document.createElement('div'));
    $goldDisplay.addClass('gold-display text-font');
    $goldDisplay.hide();

    var applySelect = function () {
        $('.menu-item-selected').removeClass('menu-item-selected');
        menuItems[selectedMenuItem].addClass('menu-item-selected');
    }

    var defaultSelect = function () {
        selectedMenuItem = 0;
        applySelect();
    }

    var selectItem = function ($menuItem) {
        switch ($menuItem.text()) {
            case 'Characters':
                FuzzyOctoTribble.CharacterScreen.showCharacterScreen();
                currentMode = "characters";
                break;
            case 'Parties':
                FuzzyOctoTribble.PartyScreen.show();
                currentMode = "parties";
                break;
            case 'Quests':
                FuzzyOctoTribble.QuestScreen.show();
                currentMode = "quests";
                break;
            case 'Configuration':
                break;
            default:
                break;
        }
    }

    that.toggleMenu = function () {
        defaultSelect();
        if ($menu.is(':visible')) {
            FuzzyOctoTribble.KeyControl.cancelMenuMode();
            FuzzyOctoTribble.CharacterScreen.hideCharacterScreen();
            FuzzyOctoTribble.PartyScreen.hide();
            FuzzyOctoTribble.QuestScreen.hide();
            currentMode = "menu";
        }
        else {
            FuzzyOctoTribble.KeyControl.setMenuMode();
        }
        $menu.toggle();
        $goldDisplay.toggle();
    }

    that.selectUp = function () {
        switch (currentMode) {
            case 'menu':
                if (selectedMenuItem != 0) {
                    selectedMenuItem--;
                    applySelect();
                }
                break;
            case 'characters':
                FuzzyOctoTribble.CharacterScreen.scrollUp();
                break;
            case 'parties':
                FuzzyOctoTribble.PartyScreen.scrollUp();
                break;
            case 'quests':
                FuzzyOctoTribble.QuestScreen.scrollUp();
                break;
        }
    }

    that.selectDown = function () {
        switch (currentMode) {
            case 'menu':
                if (selectedMenuItem != menuItems.length - 1) {
                    selectedMenuItem++;
                    applySelect();
                }
                break;
            case 'characters':
                FuzzyOctoTribble.CharacterScreen.scrollDown();
                break;
            case 'parties':
                FuzzyOctoTribble.PartyScreen.scrollDown();
                break;
            case 'quests':
                FuzzyOctoTribble.QuestScreen.scrollDown();
                break;
        }
    }

    that.selectCurrent = function () {
        selectItem(menuItems[selectedMenuItem]);
    }

    that.cancel = function () {
        switch (currentMode) {
            case 'menu':
                that.toggleMenu();
                break;
            case 'characters':
                FuzzyOctoTribble.CharacterScreen.hideCharacterScreen();
                currentMode = "menu";
                break;
            case 'parties':
                FuzzyOctoTribble.PartyScreen.hide();
                currentMode = "menu";
                break;
            case 'quests':
                FuzzyOctoTribble.QuestScreen.hide();
                currentMode = "menu";
                break;
        }
    }

    that.setPlayer = function (newPlayer) {
        $goldDisplay.text(newPlayer.gp + ' GP');
    }

    that.drawMenu = function () {
        $('.game-window').append($menu);
        $('.game-window').append($goldDisplay);
    }

    $(document).ready(function () {
        that.drawMenu();
    });

    return that;
})();