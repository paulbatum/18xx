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
    }

    public class PlayerState
    {
        public Player Player { get; }
        public int Money { get; }
        
        public PlayerState(Player player, int money)
        {
            Player = player;
            Money = money;
        }
    }
}
