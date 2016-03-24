using EighteenSeventeen.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Rounds
{
    public class StockRound : PlayerRound
    {
        public int RoundNumber { get; }
        public Auction<Location> CurrentAuction { get; }
        public override string Description => $"SR{RoundNumber}";

        public StockRound(ImmutableList<Player> players, int roundNumber, Player priority) : base(players, priority, null)
        {
            RoundNumber = roundNumber;
        }

        public override IEnumerable<IChoice> GetChoices(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
