FuzzyOctoTribble.CombatAnimation = (function () {
    var that = {};
    var animationHolders = [];

    var initAnimation = function($image) {
        var $image = $image;
        var isRight = true;
        var delayTime = Math.floor(Math.random() * 10) + 5;
        var timerTime = Math.floor(Math.random() * 200) + 50;
        var startPauseCheck = 3;
        var idleTime = 0;

        var processAnimation = function () {
            if (delayTime != 0) {
                delayTime--;
                return;
            }
            else {
                if (Math.floor(Math.random() * 12) == 0) {
                    idleTime = 6;
                    isRight = true;
                    $image.removeClass('Left');
                }
            }
            
            if (idleTime !== 0) {
                if ($image.hasClass('Idle')) {
                    $image.removeClass('Idle');
                }
                else {
                    $image.addClass('Idle');
                }

                idleTime--;
                return;
            }
            else {
                $image.removeClass('Idle');
            }

            if (startPauseCheck == 0) {
                if (Math.floor(Math.random() * 10) == 0) {
                    delayTime = Math.floor(Math.random() * 10) + 5;
                    startPauseCheck = 3;
                    if (Math.floor(Math.random() * 4) == 0) {
                        isRight = !isRight;
                        if ($image.hasClass('Left')) {
                            $image.removeClass('Left');
                        } else {
                            $image.addClass('Left');
                        }
                    }
                }
            }
            else {
                startPauseCheck--;
            }

            if ($image.width() == 0) {
                return;
            }
            if (isRight) {
                var imageWidth = $image.width();
                var imageLeft = $image.css('left');
                var parentWidth = $image.parent().width() - 40;
                if ($image.width() + parseFloat($image.css('left')) > $image.parent().width() - 5) {
                    isRight = false;
                    $image.addClass('Left');
                }
                else {
                    if ($image.hasClass('WalkOne')) {
                        $image.removeClass('WalkOne');
                    }
                    else {
                        $image.addClass('WalkOne');
                    }

                    $image.css('left', (parseFloat($image.css('left')) + 5) + 'px')
                }
            }
            else {
                if (parseFloat($image.css('left')) <= 10) {
                    isRight = true;
                    $image.removeClass('Left');
                }
                else {
                    if ($image.hasClass('WalkOne')) {
                        $image.removeClass('WalkOne');
                    }
                    else {
                        $image.addClass('WalkOne');
                    }

                    $image.css('left', (parseFloat($image.css('left')) - 5) + 'px')
                }
            }
        }

        var setAnimationTimer = function () {
            processAnimation();
            setTimeout(setAnimationTimer, timerTime);
        }

        setAnimationTimer();
    }


    that.clearAnimations = function () {
        for (var i = 0; i < animationHolders.length; i++) {
            clearTimeout(animationHolders[i].timer);
        }
    }

    that.addAnimation = function ($image) {
        initAnimation($image);

        return animationHolders.length - 1; //Return index of animation for callbacks
    }

    return that;
})();