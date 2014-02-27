FuzzyOctoTribble.QuestScreen = (function () {
    var that = {};

    var $questScreen = $(document.createElement('div'));
    $questScreen.addClass('character-screen text-font');
    $questScreen.text('Quest Screen');
    $questScreen.hide();
    $(document).ready(function () {
        $('.game-window').append($questScreen);
    });

    that.show = function () {
        $questScreen.show();
    }

    that.hide = function () {
        $questScreen.hide();
    }
    that.scrollUp = function () {

    }

    that.scrollDown = function () {

    }

    return that;
})();