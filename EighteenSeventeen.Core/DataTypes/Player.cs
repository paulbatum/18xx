using EighteenSeventeen.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.DataTypes
{
    public class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }

    public class PlayerState
    {
        public Player Player { get; }
        public int Money { get; }        
        public ImmutableList<PrivateCompany> PrivateCompanies { get; }
        
        public PlayerState(Player player, int money, ImmutableList<PrivateCompany> privateCompanies)
        {
            Player = player;
            Money = money;
            PrivateCompanies = privateCompanies;
        }

        public PlayerState(Player player, int money) : this(player, money, ImmutableList<PrivateCompany>.Empty)
        {
        }

        public int GetMoneyRoundedDownToMultipleOf(int multiple) => Money - (Money % multiple);        
    }
}
