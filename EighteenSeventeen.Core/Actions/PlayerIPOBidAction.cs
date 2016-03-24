using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighteenSeventeen.Core.DataTypes;
using EighteenSeventeen.Core.Rounds;

namespace EighteenSeventeen.Core.Actions
{
    public class PlayerIPOBidAction : GameAction, IGameAction<StockRound>
    {
        public Location Selection { get; }
        public int Bid { get; }

        public PlayerIPOBidAction(IGameAction parent, Player actingPlayer, Location selection, int bid) : base(parent, actingPlayer)
        {
            Selection = selection;
            Bid = bid;
        }

        public override bool AppliesToRound(Round round) => round is StockRound;

        public void Validate(GameState gameState, GameActionValidator validator, StockRound round)
        {
            throw new NotImplementedException();
        }

        public GameState Apply(GameState gameState, StockRound round)
        {
            throw new NotImplementedException();
        }
    }
}
