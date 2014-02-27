FuzzyOctoTribble.PartyScreen = (function () {
    var that = {};

    var $partyScreen = $(document.createElement('div'));
    $partyScreen.addClass('character-screen text-font');
    $partyScreen.text('Party Screen');
    $partyScreen.hide();
    $(document).ready(function () {
        $('.game-window').append($partyScreen);
    });

    that.show = function () {
        $partyScreen.show();
    }

    that.hide = function () {
        $partyScreen.hide();
    }
    that.scrollUp = function () {

    }

    that.scrollDown = function () {

    }

    return that;
})();