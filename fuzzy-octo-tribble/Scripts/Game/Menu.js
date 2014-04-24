FuzzyOctoTribble.Menu = function (spec, my) {
    var that = {};
    var selectedMenuItem = 0;
    var menuItems = [];
    var items = spec.items;
    var closeOnMenu = spec.closeOnMenu;
    var delayShow = spec.delayShow;
    var header = spec.header;
    if (spec.isCombat) {
        that.isCombat = true;
    }
    var additionalDisplays = spec.additionalDisplays || [];
    my = my || {};

    var selectItem = function (index) {
        items[index].selected();
    }

    var $menu = $(document.createElement('div'));
    $menu.addClass('menu-container');
    if (spec.additionalClasses) {
        $menu.addClass(spec.additionalClasses);
    }
    
    if (header) {
        var $menuTitleItem = $(document.createElement('div'));
        $menuTitleItem.addClass('menu-item menu-title text-font');
        $menuTitleItem.text(header);
        $menu.append($menuTitleItem);
    }

    for (var i = 0; i < items.length; i++) {
        var $menuItem = $(document.createElement('div'));
        $menuItem.addClass('menu-item text-font');
        var $innerText = $(document.createElement('span')).text(items[i].text).addClass('menu-item-inner-text');
        $menuItem.append($innerText);
        $menuItem.data('index', i);
        if (items[i].isDisabled) {
            $menuItem.addClass('menu-item-disabled');
        }
        else {
            $menuItem.on('click', function () {
                if (that.hasCurrentControl) {
                    items[$(this).data('index')].selected();
                }
            });
        }
        $menu.append($menuItem);
        menuItems.push($menuItem);
    }
    
    var applySelect = function () {
        $menu.find('.menu-item-selected').removeClass('menu-item-selected');
        menuItems[selectedMenuItem].addClass('menu-item-selected');
    }

    var defaultSelect = function () {
        selectedMenuItem = 0;
        applySelect();
    }

    that.show = function () {
        $('.game-window').append($menu);
        for (var i = 0; i < additionalDisplays.length; i++) {
            $('.game-window').append(additionalDisplays[i]);
        }
    }

    that.close = function () {
        that.cancel();
    }

    that.releaseUp = function () {
        var index = selectedMenuItem;
        while (index != 0) {
            index--;
            if (!menuItems[index].hasClass('menu-item-disabled')) {
                selectedMenuItem = index;
                applySelect();
                return;
            }
        }
    }

    that.releaseDown = function () {
        var index = selectedMenuItem;
        while (index != menuItems.length - 1) {
            index++;
            if (!menuItems[index].hasClass('menu-item-disabled')) {
                selectedMenuItem = index;
                applySelect();
                return;
            }
        }
    }

    that.confirm = function () {
        selectItem(selectedMenuItem);
    }

    that.cancel = function () {
        $menu.remove();
        for (var i = 0; i < additionalDisplays.length; i++) {
            additionalDisplays[i].remove();
        }
        if (that.onComplete) {
            that.onComplete();
        }
    }

    that.menu = function () {
        if (closeOnMenu) {
            that.cancel();
        }
    }

    if (!delayShow) {
        that.show();
    }

    defaultSelect();

    return that;
}

FuzzyOctoTribble.MenuHandler = (function () {
    var that = {};
    var isDungeon = false;
    var $goldDisplay = $(document.createElement('div'));
    $goldDisplay.addClass('gold-display text-font');


    var createDungeonMenu = function () {
        return FuzzyOctoTribble.Menu({
            items: [
                    {
                        text: "Characters",
                        selected: function () {
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getPartyCharacters())
                        }
                    },
                    {
                        text: "Abilities",
                        selected: function () {
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getPartyAbilityScreen())
                    }
                }
            ]
        });
    }

    var createHubMenu = function () {
        return FuzzyOctoTribble.Menu({
            items: [
            {
                text: "Characters",
                selected: function () {
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getScreen())
                }
            },
            {
                text: "Abilities",
                selected: function () {
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getAbilityScreen())
                }
            },
            {
                text: "Parties",
                selected: function () {
                    var $screen = $(document.createElement('div'));
                    $screen.text('Parties Screen');
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenControl($screen, true, function () {
                        FuzzyOctoTribble.KeyControl.menu();
                    }));
                }
            },
            {
                text: "Quests",
                selected: function () {
                    var $screen = $(document.createElement('div'));
                    $screen.text('Quests Screen');
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenControl($screen, true, function () {
                        FuzzyOctoTribble.KeyControl.menu();
                    }));
                }
            },
            {
                text: "Configuration",
                selected: function () {
                    var $screen = $(document.createElement('div'));
                    $screen.text('Configuration Screen');
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenControl($screen, true, function () {
                        FuzzyOctoTribble.KeyControl.menu();
                    }));
                }
            }
            ],
            closeOnMenu: true,
            header: "Menu",
            additionalDisplays: [$goldDisplay]
        });
    }

    that.setIsDungeon = function (isDungeonVar) {
        isDungeon = isDungeonVar;
    }

    that.setPlayer = function (newPlayer) {
        $goldDisplay.text(newPlayer.gp + ' GP');
    }

    that.getMenu = function () {
        if (isDungeon) {
            return createDungeonMenu();
        }
        else {
            return createHubMenu();
        }
    }

    return that;
})();