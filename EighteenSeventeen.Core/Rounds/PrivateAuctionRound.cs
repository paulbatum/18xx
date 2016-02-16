using EighteenSeventeen.Core.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Rounds
{
    public class PrivateAuctionRound : PlayerRound
    {
        // Needs fixing
        public const int SeedMoney = 200;

        public override string Description { get; } = "PR";
        public ImmutableList<PrivateCompany> Privates { get; }
        public Auction<PrivateCompany> CurrentAuction { get; }

        public PrivateAuctionRound(ImmutableList<PrivateCompany> privates, Auction<PrivateCompany> auction, Player activePlayer, Player lastToAct)
            : base(activePlayer, lastToAct)
        {
            Privates = privates;
            CurrentAuction = auction;
        }

        public static PrivateAuctionRound StartOfAuction(Game game)
        {
            return new PrivateAuctionRound(PrivateCompanies.All, null, game.Players.First(), game.Players.Last());
        }

        public override Round AdvanceToNextPlayer(GameState state)
        {
            return new PrivateAuctionRound(Privates, CurrentAuction, state.Game.GetPlayerAfter(this.ActivePlayer), LastToAct);
        }

        public override Round NextRound(GameState gameState)
        {
            var priorityDeal = gameState.Game.GetPlayerAfter(LastToAct);
            return new StockRound(1, priorityDeal);
        }

        public void ValidateBid(GameState gameState, GameActionValidator validator, Player player, PrivateCompany selection, int bid)
        {
            validator.ValidateMultipleOf(5, bid, "Illegal bid - must be multiple of 5");
            validator.Validate(bid < selection.Value, "Illegal bid - overbidding is not permitted");
            validator.Validate(SeedMoney >= selection.Value - bid, "Illegal bid - not enough seed money");

            var playerState = gameState.GetPlayerState(player);
            validator.Validate(bid <= playerState.Money, $"Illegal bid - cannot bid {bid} with only {playerState.Money} cash available");

            if(CurrentAuction != null)
            {                
                validator.Validate(selection == CurrentAuction.Selection, 
                    $"Illegal bid - cannot bid on private '{selection}' because there is already an auction for '{CurrentAuction.Selection}' in progress");
                validator.Validate(bid > CurrentAuction.CurrentBid, 
                    $"Illegal bid - {bid} is not greater than the current high bid of {CurrentAuction.CurrentBid}");
            }
        }

        public GameState Bid(GameState gameState, Player player, PrivateCompany selection, int bid)
        { 
            var auction = new Auction<PrivateCompany>(selection, player, bid);
            var round = new PrivateAuctionRound(Privates, auction, gameState.Game.GetPlayerAfter(player), player);
            return new GameState(gameState.Game, round, gameState.PlayerStates, gameState.CompanyStates);
        }

        public GameState Pass(GameState gameState, Player player)
        {
            Round newRound;

            if (ActivePlayer == LastToAct)
            {
                newRound = NextRound(gameState);
            }
            else
            {
                newRound = AdvanceToNextPlayer(gameState);
            }

            return new GameState(gameState.Game, newRound, gameState.PlayerStates, gameState.CompanyStates);
        }

        //public GameState Apply(GameState gameState, IGameAction gameAction)
        //{
        //    gameAction.Apply(gameState, this)
        //}
    }
}
