using EighteenSeventeen.Core.DataTypes;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Actions
{
    public class PlayerPrivateBidAction : GameAction, IGameAction<PrivateAuctionRound>
    {        
        public PrivateCompany Selection { get; }        
        public int Bid { get; }

        public PlayerPrivateBidAction(IGameAction parent, Player actingPlayer, PrivateCompany target, int bid) : base(parent, actingPlayer)
        {            
            Selection = target;
            Bid = bid;
        }

        public override bool AppliesToRound(Round round) => round is PrivateAuctionRound;

        public void Validate(GameState gameState, GameActionValidator validator, PrivateAuctionRound round)
        {
            PrivateAuctionRound.ValidateBid(validator, round, gameState.GetPlayerState(ActingPlayer), Selection, Bid);
        }

        public GameState Apply(GameState gameState, PrivateAuctionRound round)
        {
            return PrivateAuctionRound.MakeBid(gameState, round, gameState.GetPlayerState(ActingPlayer), Selection, Bid);
        }
    }   
}
