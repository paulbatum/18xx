using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class PlayerState
    {
        public Player Player { get; }
        public int Money { get; }
        public bool HasPriority { get; }
        
        public PlayerState(Player player, int money, bool hasPriority)
        {
            Player = player;
            Money = money;
            HasPriority = hasPriority;
        }

        public int GetMoneyRoundedDownToMultipleOf(int multiple)
        {
            return Money - (Money % multiple);
        }
    }
}
