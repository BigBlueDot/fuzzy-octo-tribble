FuzzyOctoTribble.OptionDialog = (function () {
    var that = {};

    var $menu = $(document.createElement('div'));
    var currentSelection = 0;
    var items = [];
    var onSelected;
    $menu.addClass('menu-container');
    $menu.hide();

    var applySelect = function () {
        $('.menu-item-selected').removeClass('menu-item-selected');
        items[currentSelection].addClass('menu-item-selected');
    }
    
    var showOptions = function (options) {
        $menu.empty();
        items = [];
        currentSelection = 0;
        FuzzyOctoTribble.KeyControl.setOptionDialogMode();

        for (var i = 0; i < options.length; i++) {
            var $menuItem = $(document.createElement('div'));
            $menuItem.addClass('menu-item text-font');
            $menuItem.text(options[i]);
            $menuItem.on('click', function () {
                onSelected($(this).text());
                FuzzyOctoTribble.KeyControl.cancelOptionDialogMode();
                $menu.hide();
            });
            $menu.append($menuItem);
            items.push($menuItem);
        }

        $menu.show();
        applySelect();
    }

    that.show = function (dialog, options, onItemSelected) {
        onSelected = onItemSelected;
        if (dialog) {
            FuzzyOctoTribble.DialogBox.showDialog(
                {
                    dialogContent: dialog,
                    onComplete: function () {
                        showOptions(options, onSelected);
                    }
                });
        }
        else {
            showOptions(options, onSelected);
        }
    }

    that.selectUp = function () {
        if (currentSelection !== 0) {
            currentSelection--;
            applySelect();
        }
    }

    that.selectDown = function () {
        if (currentSelection !== items.length - 1) {
            currentSelection++;
            applySelect();
        }
    }

    that.selectCurrent = function () {
        onSelected(items[currentSelection].text());
        $menu.hide();
        FuzzyOctoTribble.KeyControl.cancelOptionDialogMode();
    }

    that.cancel = function () {
        $menu.hide();
        FuzzyOctoTribble.KeyControl.cancelOptionDialogMode();
    }

    $(document).ready(function () {
        $('.game-window').append($menu);
    });

    return that;
})();