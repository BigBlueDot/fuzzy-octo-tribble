using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatTurnOrder
    {
        public List<CombatTurnOrderCharacter> turnOrders { get; set; }

        public CombatTurnOrder()
        {
            turnOrders = new List<CombatTurnOrderCharacter>();
        }

        public CombatTurnOrderCharacter getNext()
        {
            var returnValue = turnOrders[0];
            turnOrders.RemoveAt(0);
            return returnValue;
        }

        public void addTurnOrder(bool isPC, int characterUniq, int timeExecuted)
        {
            for (int i = 0; i < turnOrders.Count; i++)
            {
                if (turnOrders[i].timeExecuted > timeExecuted)
                {
                    turnOrders.Insert(i, new CombatTurnOrderCharacter()
                    {
                        isPC = isPC,
                        characterUniq = characterUniq,
                        timeExecuted = timeExecuted
                    });
                    break;
                }
            }
        }
    }
}
