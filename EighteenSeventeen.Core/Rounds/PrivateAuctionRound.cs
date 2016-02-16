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
            if (CurrentAuction != null)
            {
                validator.Validate(selection == CurrentAuction.Selection,
                    $"Bid on '{selection}' is not legal - there is already an auction for '{CurrentAuction.Selection}' in progress.");
                validator.Validate(bid > CurrentAuction.CurrentBid,
                    $"Bid of '{bid}' is not legal - the current high bid is '{CurrentAuction.CurrentBid}'.");
            }

            validator.ValidateMultipleOf(5, bid, $"Bid of '{bid}' is not legal - must be a multiple of 5.");
            validator.Validate(bid < selection.Value, $"Bid of '{bid}' is not legal - overbidding is not permitted.");
            validator.Validate(SeedMoney >= selection.Value - bid, "Illegal bid - not enough seed money.");

            var playerState = gameState.GetPlayerState(player);
            validator.Validate(bid <= playerState.Money, $"Bid of '{bid}' is not legal - player '{player}' has only {playerState.Money} cash available.");


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
