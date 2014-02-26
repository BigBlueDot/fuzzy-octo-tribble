$(document).ready(function () {
    $.ajax("Game/GetMap", {
        success: function (data) {
            FuzzyOctoTribble.Camera.setMap(data);
            FuzzyOctoTribble.Movement = FuzzyOctoTribble.MovementConstructor(data);
            FuzzyOctoTribble.KeyControl = FuzzyOctoTribble.KeyControlConstructor();

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