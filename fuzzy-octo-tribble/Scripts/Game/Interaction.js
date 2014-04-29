
FuzzyOctoTribble.InteractionHandler = (function () {
    var that = {};
    var map;

    that.setMap = function (newMap) {
        map = newMap;
    }

    that.getInteraction = function () {
        var x = FuzzyOctoTribble.Player.x;
        var y = FuzzyOctoTribble.Player.y;
        switch (FuzzyOctoTribble.PlayerDirection) {
            case 1:
                x -= 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                x += 1;
                break;
            case 4:
                y += 1;
                break;
            default:
                break;
        }

        if (map) {
            if (x === 0 || y === 0 || x === map.mapSquares.length || y === map.mapSquares[0].length) {
                return;
            }
            else if (!map.mapSquares[x][y].isI) {
                return;
            }
        }
        
        $.ajax("Game/GetInteraction?x=" + x + "&y=" + y, {
            success: function (data) {
                if (data.isDungeon) {
                    var dungeonItems = [];
                    var selectedDungeon = "";
                    var createDungeonItem = function (text, value) {
                        dungeonItems.push({
                            text: text,
                            selected: function () {
                                selectedDungeon = value;

                                //Will choose party here
                                if (!data.isExit) {
                                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.CharacterScreenCreator.getPartySelectScreen(
                                        data.maxPartySize,
                                        function (items) {
                                            FuzzyOctoTribble.KeyControl.cancel(); //Close out the dungeon selector
                                            if (items.length !== 0) { //Don't do anything if they didn't pick anyone for the party, just close out
                                                //Send the selected values to the server to load the dungeon:
                                                $.ajax("Game/SetDungeon?x=" + x + "&y=" + y + "&dungeonName=" + selectedDungeon + "&party=" + items.join(','),
                                                    {
                                                        success: function () {
                                                            FuzzyOctoTribble.MaintainState.updateMap();
                                                        }
                                                    });
                                            }
                                        }
                                        ));
                                }
                                else {
                                    //Send the selected values to the server to load the dungeon:
                                    FuzzyOctoTribble.KeyControl.cancel(); //Close out the selector
                                    if (selectedDungeon) {
                                        $.ajax("Game/SetDungeon?x=" + x + "&y=" + y + "&dungeonName=" + selectedDungeon + "&party=",
                                            {
                                                success: function () {
                                                    FuzzyOctoTribble.MaintainState.updateMap();
                                                }
                                            });
                                    }
                                }
                            }
                        });
                    }
                    for (var i = 0; i < data.options.length; i++) {
                        createDungeonItem(data.options[i].text, data.options[i].value);
                    }

                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox({
                        dialogContent: data.dialog,
                        onComplete: function () {
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Menu({
                                items: dungeonItems                            
                            }));
                        }
                    }));
                }
                else if (data.isClassTrainer) {
                    var currentCharacter;
                    var currentClass;
                    var classItems = [];
                    var characterItems = [];

                    var createClassItem = function (text, value) {
                        classItems.push({
                            text: text,
                            selected: function () {
                                currentClass = value;

                                $.ajax("Game/SetClass?x=" + x + "&y=" + y + "&characterName=" + currentCharacter + "&className=" + currentClass,
                                    {
                                        success: function () {
                                            FuzzyOctoTribble.KeyControl.cancel(); //Close out the dungeon selector
                                            FuzzyOctoTribble.KeyControl.cancel(); //Close out the dungeon selector
                                            FuzzyOctoTribble.MaintainState.updateMap() //Get latest version of player
                                        }
                                    });
                            }
                        });
                    }

                    var createCharacterItem = function (text, value) {
                        characterItems.push(
                            {
                                text: text,
                                selected: function () {
                                    currentCharacter = value;
                                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Menu({
                                        items: classItems,
                                        closeOnMenu: true
                                    }));
                                }
                            });
                    }

                    for (var i = 0; i < data.options.length; i++) {
                        createCharacterItem(data.options[i].text, data.options[i].value);
                    }

                    for (var i = 0; i < data.classes.length; i++) {
                        createClassItem(data.classes[i].text, data.classes[i].value);
                    }

                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox({
                        dialogContent: data.dialog,
                        onComplete: function () {
                            FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.Menu({
                                items: characterItems,
                                closeOnMenu:true
                            }));
                        }
                    }));
                }
                else if (data.hasDialog) {
                    FuzzyOctoTribble.KeyControl.addController(FuzzyOctoTribble.DialogBox(
                        {
                            dialogContent: data.dialog
                        }));
                }
            }
        });
    }

    return that;
})();