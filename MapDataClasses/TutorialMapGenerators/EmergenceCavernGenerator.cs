using EnemyDataClasses.Goblins;
using MapDataClasses.EventClasses;
using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.TutorialMapGenerators
{
    public class EmergenceCavernGenerator : IVillageGenerator
    {
        private static EmergenceCavernGenerator _implementation;
        public static EmergenceCavernGenerator Implementation
        {
            get
            {
                if (_implementation == null)
                {
                    _implementation = new EmergenceCavernGenerator();
                }

                return _implementation;
            }
        }

        public MapModel getMap()
        {
            MapModel mm = new MapModel();
            mm.name = "Emergence Cavern";
            mm.map = new string[50, 40];
            mm.startX = 26;
            mm.startY = 37;
            for (var x = 0; x < 50; x++)
            {
                for (var y = 0; y < 40; y++)
                {
                    mm.map[x, y] = "CaveEmpty";
                }
                mm.map[x, 0] = "CaveWall";
                mm.map[x, 39] = "CaveWall";
            }

            for (var y = 0; y < 40; y++)
            {
                mm.map[0, y] = "CaveWall";
                mm.map[49, y] = "CaveWall";
            }

            //Draw walls
            for (var x = 0; x < 25; x++)
            {
                mm.map[x, 34] = "CaveWall";
            }

            for (var x = 28; x < 44; x++)
            {
                mm.map[x, 34] = "CaveWall";
            }

            for (var y = 34; y >= 25; y--)
            {
                mm.map[24, y] = "CaveWall";
                mm.map[28, y] = "CaveWall";
                if (y % 2 == 0)
                {
                    mm.map[23, y] = "CaveWall";
                    mm.map[29, y] = "CaveWall";
                }
            }

            for (var x = 24; x < 28; x++)
            {
                mm.map[x, 25] = "CaveWall";
            }

            for (var y = 34; y >= 10; y--)
            {
                mm.map[42, y] = "CaveWall";
                mm.map[47, y] = "CaveWall";
                if (y % 2 == 0)
                {
                    mm.map[41, y] = "CaveWall";
                    mm.map[48, y] = "CaveWall";
                }
            }
            for (var x = 42; x < 47; x++)
            {
                mm.map[x, 10] = "CaveWall";
            }

            mm.map[26, 38] = "Exit";

            mm.eventCollection = new MapEventCollectionModel();
            IEventData eventData = EventHolder.getMapEvent(1);
            mm.eventCollection.addEvent(new MapEventModel()
            {
                x = 26,
                y = 27,
                rewardType = ClientEvent.RewardType.Objective,
                eventData = eventData
            });

            return mm;
        }

        public MapModel getMapFloor2()
        {
            MapModel mm = new MapModel();
            mm.name = "Emergence Cavern F2";
            mm.map = new string[50, 40];
            mm.startX = 26;
            mm.startY = 37;
            for (var x = 0; x < 50; x++)
            {
                for (var y = 0; y < 40; y++)
                {
                    mm.map[x, y] = "CaveEmpty";
                }
                mm.map[x, 0] = "CaveWall";
                mm.map[x, 39] = "CaveWall";
            }

            for (var y = 0; y < 40; y++)
            {
                mm.map[0, y] = "CaveWall";
                mm.map[49, y] = "CaveWall";
            }

            //Draw walls
            for (var x = 0; x < 25; x++)
            {
                mm.map[x, 34] = "CaveWall";
            }

            for (var x = 28; x < 44; x++)
            {
                mm.map[x, 34] = "CaveWall";
            }

            for (var y = 34; y >= 25; y--)
            {
                mm.map[24, y] = "CaveWall";
                mm.map[28, y] = "CaveWall";
                if (y % 2 == 0)
                {
                    mm.map[23, y] = "CaveWall";
                    mm.map[29, y] = "CaveWall";
                }
            }

            for (var x = 24; x < 28; x++)
            {
                mm.map[x, 25] = "CaveWall";
            }

            for (var y = 34; y >= 10; y--)
            {
                mm.map[42, y] = "CaveWall";
                mm.map[47, y] = "CaveWall";
                if (y % 2 == 0)
                {
                    mm.map[41, y] = "CaveWall";
                    mm.map[48, y] = "CaveWall";
                }
            }
            for (var x = 42; x < 47; x++)
            {
                mm.map[x, 10] = "CaveWall";
            }

            mm.map[26, 38] = "Exit";

            return mm;
        }

        public MapInteraction getInteraction(MapModel mm, int x, int y)
        {
            MapInteraction mi = new MapInteraction();
            if (mm.map[x, y] == "Exit")
            {
                mi = new DungeonSelectInteraction();
                mi.hasDialog = true;
                mi.hasOptions = true;
                mi.dialog = "Would you like to leave this dungeon?";
                mi.options = new List<MapOption>();
                mi.options.Add(new MapOption() { text = "Yes", value = "Ensemble Village" });
                mi.options.Add(new MapOption() { text = "No", value = "" });
                ((DungeonSelectInteraction)mi).isExit = true;
            }
            return mi;
        }

        public bool validateDungeonSelection(MapModel mm, int x, int y, string selectedDungeon)
        {
            if (mm.map[x, y] == "Exit")
            {
                if (selectedDungeon == "Ensemble Village")
                {
                    return true;
                }
            }

            return false;
        }

        public void performInteraction(MapModel mm, int x, int y, string selectedOption)
        {
            //Do logic for determining what to do
            //This may need to be event driven (i.e. fire onRemoveGold and onRestoreHealth and onRestoreMP
            if (mm.map[x, y] == "Exit")
            {
                if (selectedOption == "Yes")
                {

                }
            }
        }

        public int getMinCombatCount()
        {
            return 7;
        }

        public int getMaxCombatCount()
        {
            return 14;
        }

        public int getRandomEncounterCount()
        {
            return 1;
        }

        public int getRandomEncounterCountF2()
        {
            return 3;
        }

        public Encounter getRandomEncounter(int selection)
        {
            Encounter encounter = new Encounter();
            encounter.enemies = new List<MapDataClasses.Enemy>();
            encounter.enemies.Add(getEnemy("Goblin"));
            encounter.message = "A lone goblin appears!";
            return encounter;
        }

        public Encounter getRandomEncounterF2(int selection)
        {
            Encounter encounter = new Encounter();
            encounter.enemies = new List<MapDataClasses.Enemy>();
            switch(selection)
            {
                case 1:
                    encounter.message = "A horde of goblins has appeared!";
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Goblin"));
                    break;
                case 2:
                    encounter.message = "A horde of goblins has appeared!";
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Goblin"));
                    break;
                case 3:
                    encounter.message = "A goblin in a suit and his assistant appear!";
                    encounter.enemies.Add(getEnemy("Goblin"));
                    encounter.enemies.Add(getEnemy("Boss Goblin"));
                    break;
            }
            return encounter;
        }

        public Enemy getEnemy(string enemyType)
        {
            switch (enemyType)
            {
                case "Goblin":
                    return new MapDataClasses.Enemy() { name = EnemyDataClasses.Goblins.NameGenerator.getGoblinName(), maxHP = 25, maxMP = 1, strength = 5, vitality = 5, agility = 5, intellect = 5, wisdom = 5, level = 1, type = "Goblin", xp = 5, cp = 5 };
                case "Boss Goblin":
                    return new Enemy() { name = EnemyDataClasses.Goblins.NameGenerator.getGoblinName(), maxHP = 50, maxMP = 1, strength = 10, vitality = 10, agility = 10, intellect = 10, wisdom = 10, level = 3, type = "Boss Goblin", xp = 10, cp = 10 };
                default:
                    return getEnemy("Goblin");
            }
        }
    }
}
