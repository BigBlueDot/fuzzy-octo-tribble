﻿$(document).ready(function () {
    $.ajax("Game/GetMap", {
        success: function (data) {
            FuzzyOctoTribble.Camera.setMap(data);

            $.ajax("Game/GetPlayer", {
                success: function (data) {
                    FuzzyOctoTribble.Player = data;
                    FuzzyOctoTribble.Camera.setPlayer(FuzzyOctoTribble.Player);
                    FuzzyOctoTribble.Camera.draw();
                }
            });
        }
    });
});