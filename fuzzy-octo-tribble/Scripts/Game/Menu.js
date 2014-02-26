FuzzyOctoTribble.Menu = (function () {
    var that = {};

    var $menu = $(document.createElement('div'));
    $menu.addClass('menu-container');

    var $menuTitleItem = $(document.createElement('div'));
    $menuTitleItem.addClass('menu-item menu-title text-font');
    $menuTitleItem.text('Menu');
    
    $menu.append($menuTitleItem);
    $menu.hide();

    that.toggleMenu = function () {
        if ($menu.is(':visible')) {
            FuzzyOctoTribble.KeyControl.cancelMenuMode();
        }
        else {
            FuzzyOctoTribble.KeyControl.setMenuMode();
        }
        $menu.toggle();
    }

    that.drawMenu = function () {
        $('.game-window').append($menu);
    }

    return that;
})();