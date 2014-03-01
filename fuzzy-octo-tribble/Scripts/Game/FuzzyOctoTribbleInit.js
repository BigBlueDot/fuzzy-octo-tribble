$(document).ready(function () {
    $.ajax("Game/GetMap", {
        success: function (data) {
            FuzzyOctoTribble.Camera.setMap(data);
            FuzzyOctoTribble.InteractionHandler.setMap(data);
            FuzzyOctoTribble.Movement = FuzzyOctoTribble.MovementConstructor(data);
            FuzzyOctoTribble.KeyControl = FuzzyOctoTribble.KeyControlConstructor();
            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Movement);

            //Example Dialog
            //FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox("This is some text, let's see if it works"));

            //Example OptionDialog
            //FuzzyOctoTribble.OptionDialog.show("HEY ARE YOU READY TO START THE GAME WHAT WHAT", ['Something', 'Bob', 'Dungoen 3'], function (selected) { alert(selected); });

            FuzzyOctoTribble.MaintainState.start();
        }
    });
});