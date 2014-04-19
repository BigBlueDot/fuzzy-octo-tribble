FuzzyOctoTribble.ScreenMultiSelectControl = function (spec, my) {
    var subMy = {};
    var currentCount = 0;
    var items = spec.items;
    var closeOnMenu = spec.closeOnMenu;
    var onCloseMenu = spec.onCloseMenu;
    var onSelectComplete = spec.onSelectComplete;
    var selectedItems = [];
    my = my || {};
    for (var i = 0; i < items.length; i++) {
        var currentIndex = i;
        items[i].content.data('index', i);
        items[i].select = function () {
            if ($(this).hasClass('popup-screen-item-checked')) {
                $(this).removeClass('popup-screen-item-checked');
                selectedItems[$(this).data('index')] = false;
                currentCount--;
            }
            else {
                if (currentCount < spec.maxSelectCount) {
                    $(this).addClass('popup-screen-item-checked');
                    selectedItems[$(this).data('index')] = true;
                    currentCount++;
                }
            }
        }
    }

    var $done = $(document.createElement('div'));
    $done.text("Done");
    $done.addClass('text-font multi-select-done');
    $('.game-window').append($done);

    $done.click(function (e) {
        that.hideScreen();
        subMy.cancel();
        if (onSelectComplete) {
            var currentItems = [];
            for (var i = 0; i < selectedItems.length; i++) {
                if (selectedItems[i]) {
                    currentItems.push(items[i].value);
                }
            }
            onSelectComplete(currentItems);
        }
    });

    subMy.cancel = function () {
        $done.remove();
    }

    var that = FuzzyOctoTribble.ScreenSelectControl(spec, subMy);

    var localUp = that.releaseUp;
    var localDown = that.releaseDown;
    var storeConfirm = that.confirm;
    var storeSelected;
    var isRight = false;
    that.releaseRight = function () {
        if (!isRight) {
            isRight = true;
            storeSelected = $('.popup-screen-item-selected');
            storeSelected.removeClass('popup-screen-item-selected');
            $done.addClass('popup-screen-item-selected');
            that.confirm = function () {
                $done.click();
            }
        }
    }

    that.releaseLeft = function () {
        that.confirm = storeConfirm;
        isRight = false;
        if (storeSelected) {
            storeSelected.addClass('popup-screen-item-selected');
        }
        $done.removeClass('popup-screen-item-selected');
    }
    
    that.releaseDown = function () {
        if (!isRight) {
            localDown();
        }
    }

    that.releaseUp = function () {
        if (!isRight) {
            localUp();
        }
    }

    return that;
}