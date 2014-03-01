$(document).ready(function () {
    $.ajax("Game/GetMap", {
        success: function (data) {
            FuzzyOctoTribble.Camera.setMap(data);
            FuzzyOctoTribble.InteractionHandler.setMap(data);
            FuzzyOctoTribble.Movement = FuzzyOctoTribble.MovementConstructor(data);
            FuzzyOctoTribble.KeyControl = FuzzyOctoTribble.KeyControlConstructor();
            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Movement);

            //Example ScreenMultiSelect
            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenMultiSelectControl({
                items: [
                    {
                        content: $(document.createElement('div')).text('Option 1'),
                        value: 'Option 1'
                    },
                    {
                        content: $(document.createElement('div')).text('Option 2'),
                            value: 'Option 2'
                    },
                    {
                        content: $(document.createElement('div')).text('Option 3'),
                            value: 'Option 3'
                    },
                    {
                        content: $(document.createElement('div')).text('Option 4'),
                        value: 'Option 4'
                    }
                ],
                maxSelectCount: 2,
                onSelectComplete: function (items) {
                    alert(items.join(', '));
                }
            }));

            //Example ScreenSelect
            //FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenSelectControl({items:[
            //    {
            //        content: $(document.createElement('div')).text('Option 1'),
            //        select: function () {
            //            alert("Option 1 selected");
            //        }
            //    },
            //    {
            //        content: $(document.createElement('div')).text('Option 2'),
            //        select: function () {
            //            alert("Option 2 selected");
            //        }
            //    }
            //    ]}));

            //Example Display Screen
            //var $questScreen = $(document.createElement('div'));
            //$questScreen.text('Quest Screen');
            //FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.ScreenControl($questScreen));

            //Example Menu
            //FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Menu([{
            //    text: "Option 1",
            //    selected: function () {
            //        alert("Option 1");
            //    }
            //}, {
            //    text: "Option 2",
            //    selected: function () {
            //        alert("Option 2");
            //    }
            //}, {
            //    text: "Option 3",
            //    selected: function () {
            //        alert("Option 3");
            //    }
            //}], true, 'Menu Header'));

            //Example Dialog
            //FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox("This is some text, let's see if it works.  I am going to add a lot more text here so that it will have to do a next button since I did not test this properly before.  Hopefully it works. :)  But man, it requires a lot of text before it moves onto the next page whenever the screen is large.  I think I will just type random characters and see what happens. Okay?  Here goes: ;lkasdjhf;oiahs;ogih a;lskdfjlk ahsdjg kahsgl khaldjgkn la;jhgl;ks ahdfvjs ad;lfas;lkd fh;lska jdf klansdfk jashdfl kjh"));

            //Example OptionDialog
            //FuzzyOctoTribble.OptionDialog.show("HEY ARE YOU READY TO START THE GAME WHAT WHAT", ['Something', 'Bob', 'Dungoen 3'], function (selected) { alert(selected); });

            FuzzyOctoTribble.MaintainState.start();
        }
    });
});