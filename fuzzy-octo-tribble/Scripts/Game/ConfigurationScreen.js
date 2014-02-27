FuzzyOctoTribble.ConfigurationScreen = (function () {
    var that = {};

    var $configurationScreen = $(document.createElement('div'));
    $configurationScreen.addClass('character-screen text-font');
    $configurationScreen.text('Configuration Screen');
    $configurationScreen.hide();
    $(document).ready(function () {
        $('.game-window').append($configurationScreen);
    });

    that.show = function () {
        $configurationScreen.show();
    }

    that.hide = function () {
        $configurationScreen.hide();
    }
    that.scrollUp = function () {

    }

    that.scrollDown = function () {

    }

    return that;
})();