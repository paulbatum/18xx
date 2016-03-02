using EighteenSeventeen.Core.DataTypes;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Actions
{
    public class PlayerPassAction : GameAction, IGameAction<PrivateAuctionRound>
    {
        public PlayerPassAction(IGameAction parent, Player actingPlayer) : base(parent, actingPlayer)
        {
            
        }

        public override bool AppliesToRound(Round round)
        {
            return round is PrivateAuctionRound;
        }

        public void Validate(GameState gameState, GameActionValidator validator, PrivateAuctionRound round)
        {
            // No validation required
        }

        public GameState Apply(GameState gameState, PrivateAuctionRound round)
        {
            return round.Pass(gameState, ActingPlayer);
        }                
    }
}
