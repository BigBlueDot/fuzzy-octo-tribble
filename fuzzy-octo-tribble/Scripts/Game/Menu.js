﻿FuzzyOctoTribble.Menu = function (spec, my) {
    var that = {};
    var selectedMenuItem = 0;
    menuItems = [];
    var items = spec.items;
    var closeOnMenu = spec.closeOnMenu;
    var header = spec.header;
    var additionalDisplays = spec.additionalDisplays || [];
    my = my || {};

    var selectItem = function(index) {
        items[index].selected();
    }

    var $menu = $(document.createElement('div'));
    $menu.addClass('menu-container');
    
    if (header) {
        var $menuTitleItem = $(document.createElement('div'));
        $menuTitleItem.addClass('menu-item menu-title text-font');
        $menuTitleItem.text(header);
        $menu.append($menuTitleItem);
    }

    for (var i = 0; i < items.length; i++) {
        var $menuItem = $(document.createElement('div'));
        $menuItem.addClass('menu-item text-font');
        $menuItem.text(items[i].text);
        $menuItem.data('index', i);
        $menuItem.on('click', function () { items[$(this).data('index')].selected(); });
        $menu.append($menuItem);
        menuItems.push($menuItem);
    }

    $('.game-window').append($menu);
    for (var i = 0; i < additionalDisplays.length; i++) {
        $('.game-window').append(additionalDisplays[i]);
    }
    
    var applySelect = function () {
        $('.menu-item-selected').removeClass('menu-item-selected');
        menuItems[selectedMenuItem].addClass('menu-item-selected');
    }

    var defaultSelect = function () {
        selectedMenuItem = 0;
        applySelect();
    }

    that.releaseUp = function () {
        if (selectedMenuItem != 0) {
            selectedMenuItem--;
            applySelect();
        }
    }

    that.releaseDown = function () {
        if (selectedMenuItem != menuItems.length - 1) {
            selectedMenuItem++;
            applySelect();
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

    defaultSelect();

    return that;
}

FuzzyOctoTribble.MenuHandler = (function () {
    var that = {};
    var $goldDisplay = $(document.createElement('div'));
    $goldDisplay.addClass('gold-display text-font');

    that.setPlayer = function (newPlayer) {
        $goldDisplay.text(newPlayer.gp + ' GP');
    }

    that.createHubMenu = function () {

        return FuzzyOctoTribble.Menu({
            items:[
            {
                text: "Characters",
                selected: function () {
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getScreen())
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

    return that;
})();