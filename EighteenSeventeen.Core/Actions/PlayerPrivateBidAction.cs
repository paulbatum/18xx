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
        public Player Player { get; }
        public PrivateCompany Target { get; }        
        public int Bid { get; }

        public PlayerPrivateBidAction(Player player, PrivateCompany target, int bid)
        {
            Player = player;
            Target = target;
            Bid = bid;
        }

        //public IEnumerable<IGameActionConstraint> DefineConstraints(GameActionConstraintsBuilder constraints)
        //{
        //    return constraints
        //        .CurrentRoundOfType<PrivateAuctionRound>()


        //    //var constraints = new List<IGameActionConstraint>();
        //    constraints.Add(GameActionConstraints.CurrentRoundOfType<PrivateAuctionRound>())
        //}


        //public override GameActionValidationResult Validate(GameState gameState, GameActionValidator validator)
        //{
        //    validator.ValidateCurrentRoundOfType<PrivateAuctionRound>();
        //    validator.ValidateMultipleOf(5, Bid, "Illegal bid - must be multiple of 5");
        //    validator.Validate(Bid > Target.Value, "Illegal bid - overbidding is not permitted");


        //    var playerState = gameState.GetPlayerState(Player);
        //    validator.Validate(Bid <= playerState.Money, $"Illegal bid - cannot bid {Bid} with only {playerState.Money} cash available");

        //}

        //public override GameActionValidationResult Validate(GameState gameState)
        //{
        //    return gameState.Round.ValidateGameAction(gameState, this);
        //}

        //public override GameState Apply(GameState gameState)
        //{
        //    var privateRound = (PrivateAuctionRound)gameState.Round;
        //    return privateRound.Bid(gameState, Player, Target, Bid);
        //    //return gameState.Round.ApplyGameAction(gameState, this);            
        //}

        public override bool AppliesToRound(Round round)
        {
            return round is PrivateAuctionRound;
        }

        public void Validate(GameState gameState, GameActionValidator validator, PrivateAuctionRound round)
        {
            round.ValidateBid(gameState, validator, Player, Target, Bid);
        }

        public GameState Apply(GameState gameState, PrivateAuctionRound round)
        {
            return round.Bid(gameState, Player, Target, Bid);
        }
    }

    public class PlayerPrivateBidChoice
    {
        public Player Player { get; }
        public ImmutableList<PrivateCompany> AvailablePrivates { get; }       
    }
}
