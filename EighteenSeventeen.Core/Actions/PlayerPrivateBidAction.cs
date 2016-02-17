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
        public PrivateCompany Target { get; }        
        public int Bid { get; }

        public PlayerPrivateBidAction(IGameAction parent, Player actingPlayer, PrivateCompany target, int bid) : base(parent, actingPlayer)
        {            
            Target = target;
            Bid = bid;
        }

        public override bool AppliesToRound(Round round)
        {
            return round is PrivateAuctionRound;
        }

        public void Validate(GameState gameState, GameActionValidator validator, PrivateAuctionRound round)
        {
            round.ValidateBid(gameState, validator, ActingPlayer, Target, Bid);
        }

        public GameState Apply(GameState gameState, PrivateAuctionRound round)
        {
            return round.Bid(gameState, ActingPlayer, Target, Bid);
        }
    }

    //public class PlayerPrivateBidChoice
    //{
    //    public Player Player { get; }
    //    public ImmutableList<PrivateCompany> AvailablePrivates { get; }       
    //}
}
