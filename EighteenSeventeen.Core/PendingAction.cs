using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class PendingAction
    {
        public Player ActivePlayer { get; }
        public ImmutableList<IChoice> Choices { get; }

        public PendingAction(Player player, ImmutableList<IChoice> choices)
        {
            ActivePlayer = player;
            Choices = choices;
        }
    }
}
