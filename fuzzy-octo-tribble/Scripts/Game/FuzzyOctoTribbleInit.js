$(document).ready(function () {
    $.ajax("Game/GetMap", {
        success: function (data) {
            FuzzyOctoTribble.Camera.setMap(data);
            FuzzyOctoTribble.Movement = FuzzyOctoTribble.MovementConstructor(data);
            FuzzyOctoTribble.KeyControl = FuzzyOctoTribble.KeyControlConstructor();

            FuzzyOctoTribble.MaintainState.start();
        }
    });
});