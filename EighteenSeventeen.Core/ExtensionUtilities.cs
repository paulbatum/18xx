using EighteenSeventeen.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public static class ExtensionUtilities
    {        
        public static Player GetPlayerAfter(this ImmutableList<Player> players, Player p) => players[(players.IndexOf(p) + 1) % players.Count];
    }
}
