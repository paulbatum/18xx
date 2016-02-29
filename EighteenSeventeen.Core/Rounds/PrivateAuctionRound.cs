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
        public int SeedMoney { get; }
        public override string Description { get; } = "PR";
        public ImmutableList<PrivateCompany> Privates { get; }
        public PrivateCompanyAuction CurrentAuction { get; }

        public PrivateAuctionRound(ImmutableList<PrivateCompany> privates, PrivateCompanyAuction auction, Player activePlayer, Player lastToAct, int seedMoney)
            : base(activePlayer, lastToAct)
        {
            Privates = privates;
            CurrentAuction = auction;
            SeedMoney = seedMoney;
        }

        public static PrivateAuctionRound StartOfAuctionRound(Game game)
        {
            return new PrivateAuctionRound(PrivateCompanies.All, null, game.Players.First(), game.Players.Last(), 200);
        }

        public void ValidateBid(GameState gameState, GameActionValidator validator, Player player, PrivateCompany selection, int bid)
        {
            if (CurrentAuction != null)
            {
                validator.Validate(selection == CurrentAuction.Selection,
                    $"Bid on '{selection}' is not legal - there is already an auction for '{CurrentAuction.Selection}' in progress.");
                validator.Validate(bid > CurrentAuction.HighBid,
                    $"Bid of '{bid}' is not legal - the current high bid is '{CurrentAuction.HighBid}'.");
            }

            validator.ValidateMultipleOf(5, bid, $"Bid of '{bid}' is not legal - must be a multiple of 5.");
            validator.Validate(bid <= selection.Value, $"Bid of '{bid}' is not legal - overbidding is not permitted.");
            validator.Validate(SeedMoney >= selection.Value - bid, "Illegal bid - not enough seed money.");

            var playerState = gameState.GetPlayerState(player);
            validator.Validate(bid <= playerState.Money, $"Bid of '{bid}' is not legal - player '{player}' has only {playerState.Money} cash available.");
        }

        public GameState Bid(GameState gameState, Player biddingPlayer, PrivateCompany selection, int bid)
        {
            if (CurrentAuction != null)
            {
                var nextPlayer = CurrentAuction.GetPlayerAfter(biddingPlayer);
                if (bid == selection.Value)
                {
                    // We need to use the same logic as for when the last player passes
                    throw new NotImplementedException();
                }
                else
                {
                    var auction = new PrivateCompanyAuction(selection, biddingPlayer, bid, CurrentAuction.Participants);
                    var round = new PrivateAuctionRound(Privates, auction, nextPlayer, LastToAct, SeedMoney);
                    return new GameState(gameState.Game, round, gameState.PlayerWithPriority, gameState.PlayerStates, gameState.CompanyStates);
                }
            }
            else
            {
                var auction = new PrivateCompanyAuction(selection, biddingPlayer, bid, gameState.Game.Players);
                var round = new PrivateAuctionRound(Privates, auction, gameState.Game.GetPlayerAfter(biddingPlayer), biddingPlayer, SeedMoney);
                return new GameState(gameState.Game, round, gameState.PlayerWithPriority, gameState.PlayerStates, gameState.CompanyStates);
            }
        }

        public GameState Pass(GameState gameState, Player actingPlayer)
        {
            Round newRound;
            var newPlayerStates = gameState.PlayerStates;

            if (CurrentAuction != null)
            {
                var nextPlayer = CurrentAuction.GetPlayerAfter(actingPlayer);                

                if (nextPlayer == CurrentAuction.HighBidder)
                {
                    // auction is over
                    var stateForWinningPlayer = gameState.GetPlayerState(CurrentAuction.HighBidder);
                    var winningPlayerPrivates = stateForWinningPlayer.PrivateCompanies.Add(CurrentAuction.Selection);

                    newPlayerStates = newPlayerStates.Remove(stateForWinningPlayer);                    
                    newPlayerStates = newPlayerStates.Add(new PlayerState(stateForWinningPlayer.Player, stateForWinningPlayer.Money - CurrentAuction.HighBid, winningPlayerPrivates));

                    var seedFunding = CurrentAuction.Selection.Value - CurrentAuction.HighBid;

                    newRound = new PrivateAuctionRound(Privates.Remove(CurrentAuction.Selection), null, gameState.Game.GetPlayerAfter(LastToAct), LastToAct, SeedMoney - seedFunding);
                }
                else
                {
                    var newAuction = new PrivateCompanyAuction(CurrentAuction.Selection, CurrentAuction.HighBidder, CurrentAuction.HighBid, CurrentAuction.Participants.Remove(actingPlayer));
                    newRound = new PrivateAuctionRound(Privates, newAuction, nextPlayer, LastToAct, SeedMoney);
                }
            }            
            else if (ActivePlayer == LastToAct)
            {
                var priorityDeal = gameState.Game.GetPlayerAfter(LastToAct);                
                newRound = new StockRound(1, priorityDeal);
            }
            else
            {
                newRound = new PrivateAuctionRound(Privates, CurrentAuction, gameState.Game.GetPlayerAfter(ActivePlayer), LastToAct, SeedMoney);
            }

            return new GameState(gameState.Game, newRound, gameState.PlayerWithPriority, newPlayerStates, gameState.CompanyStates);
        }

        public override IEnumerable<IChoice> GetChoices(GameState gameState)
        {
            var choices = new List<IChoice>();
            choices.Add(new PassChoice());

            var playerState = gameState.GetPlayerState(ActivePlayer);

            if (CurrentAuction != null)
            {                    
                choices.Add(GetLegalBid(playerState, CurrentAuction.Selection));
            }
            else
            {                
                foreach (var company in Privates)
                    choices.Add(GetLegalBid(playerState, company));
            }

            return choices.Where(c => c != null);
        }

        private BidChoice<PrivateCompany> GetLegalBid(PlayerState activePlayerState, PrivateCompany selection)
        {           
            var currentBid = this.CurrentAuction?.HighBid ?? 0;
            var min = Math.Max(selection.Value - SeedMoney, currentBid + 5);
            var max = Math.Min(selection.Value, activePlayerState.GetMoneyRoundedDownToMultipleOf(5));
            
            if (min > max)
                return null;

            return new BidChoice<PrivateCompany>(selection, min, max);
        }

        public class PrivateCompanyAuction : Auction<PrivateCompany>
        {
            public PrivateCompanyAuction(PrivateCompany selection, Player highBidder, int currentBid, ImmutableList<Player> participants)
                : base (selection, highBidder, currentBid, participants)
            {

            }

            //internal GameState Pass(GameState gameState, Player player, PrivateAuctionRound privateAuctionRound)
            //{
            //    var nextPlayer = gameState.Game.GetPlayerAfter(player);
            //    if(nextPlayer == HighBidder)
            //    {
            //        // Auction is over
            //    }
            //}
        }
    }
}
