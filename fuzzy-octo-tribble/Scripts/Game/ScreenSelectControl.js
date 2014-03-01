FuzzyOctoTribble.ScreenSelectControl = function (spec, my) {
    var that = {};
    var items = spec.items;
    var closeOnMenu = spec.closeOnMenu;
    var onCloseMenu = spec.onCloseMenu;

    $screenSelect = $(document.createElement('div'));
    $screenSelect.addClass('popup-screen');
    $('.game-window').append($screenSelect);
    var screenItems = [];
    var currentSelection = 0;

    var applySelect = function () {
        $('popup-screen-item-selected').removeClass('popup-screen-item-selected');
        screenItems[currentSelection].addClass('popup-screen-item-selected');
    }

    for (var i = 0; i < items.length; i++) {
        items[i].content.addClass('text-font popup-screen-item');
        $screenSelect.append(items[i].content);
        items[i].content.click(items[i].select);
        screenItems.push(items[i].content);
    }

    var scrollDownOne = function () {
        for (var i = 0; i < screenItems.length; i++) {
            if (screenItems[i].is(":visible")) {
                screenItems[i].hide();
                break;
            }
        }
    }

    var scrollUpOne = function () {
        if (screenItems[0].is(':visible')) {
            return;
        }
        for (var i = 0; i < screenItems.length; i++) {
            if (screenItems[i].is(":visible")) {
                screenItems[i - 1].show();
                break;
            }
        }
    }

    that.releaseUp = function () {
        if (currentSelection == 0) {
            return;
        }

        $('.popup-screen-item-selected').removeClass('popup-screen-item-selected');
        currentSelection--;
        screenItems[currentSelection].addClass('popup-screen-item-selected');
        if (!screenItems[currentSelection].is(":visible")) {
            scrollUpOne();
        }
    }

    that.releaseDown = function () {
        if (currentSelection == screenItems.length - 1) {
            return;
        }

        $('.popup-screen-item-selected').removeClass('popup-screen-item-selected');
        currentSelection++;
        screenItems[currentSelection].addClass('popup-screen-item-selected');
        if (screenItems[currentSelection].position().top >= $screenSelect.height()) {
            scrollDownOne();
        }
    }

    that.confirm = function () {
        screenItems[currentSelection].click();
    }

    that.cancel = function () {
        $screenSelect.remove();
        if (my.cancel) {
            my.cancel();
        }
        that.onComplete();
    }
    
    that.menu = function () {
        if (closeOnMenu) {
            that.cancel();
            onCloseMenu();
        }
    }

    that.hideScreen = function () {
        $screenSelect.remove();
        that.onComplete();
    }

    applySelect();

    return that;
}