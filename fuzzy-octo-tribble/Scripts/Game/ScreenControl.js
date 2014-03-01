FuzzyOctoTribble.ScreenControl = function ($content, closeOnMenu, onCloseMenu) {
    var that = {};
    $content.addClass('popup-screen text-font')

    $('.game-window').append($content);

    that.cancel = function () {
        $content.remove();
        that.onComplete();
    }

    that.menu = function () {
        if (closeOnMenu) {
            that.cancel();
            onCloseMenu();
        }
    }

    return that;
}