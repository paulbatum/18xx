using EighteenSeventeen.Core.Actions;
using EighteenSeventeen.Core.DataTypes;
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
        public ImmutableList<Player> Players { get; }
        public PrivateCompanyAuction CurrentAuction { get; }

        public PrivateAuctionRound(ImmutableList<Player> players, ImmutableList<PrivateCompany> privates, PrivateCompanyAuction auction, Player activePlayer, Player lastToAct, int seedMoney)
            : base(activePlayer, lastToAct)
        {
            Players = players;
            Privates = privates;
            CurrentAuction = auction;
            SeedMoney = seedMoney;
        }

        public static PrivateAuctionRound StartOfRound(ImmutableList<Player> players) => 
            new PrivateAuctionRound(players, PrivateCompanies.All, null, players.First(), players.Last(), 200);

        //public PrivateAuctionRound WithActivePlayer(Player activePlayer) => new PrivateAuctionRound(Privates, CurrentAuction, activePlayer, LastToAct, SeedMoney);
        private PrivateAuctionRound Update(ImmutableList<PrivateCompany> privates = null, PrivateCompanyAuction auction = null, Player activePlayer = null, Player lastToAct = null, int? seedMoney = null) =>
            new PrivateAuctionRound(Players, privates ?? Privates, auction ?? CurrentAuction, activePlayer ?? ActivePlayer, lastToAct ?? LastToAct, seedMoney ?? SeedMoney);        

        public PrivateAuctionRound StartAuction(Player biddingPlayer, PrivateCompany selection, int bid)
        {
            var auction = new PrivateCompanyAuction(selection, biddingPlayer, bid, Players);
            return new PrivateAuctionRound(Players, Privates, auction, Players.GetPlayerAfter(biddingPlayer), biddingPlayer, SeedMoney);            
        }

        public static void ValidateBid(GameActionValidator validator, PrivateAuctionRound round, PlayerState actingPlayerState, PrivateCompany selection, int bid)
        {
            var currentAuction = round.CurrentAuction;
            if (currentAuction != null)
            {
                validator.Validate(selection == currentAuction.Selection,
                    $"Bid on '{selection}' is not legal - there is already an auction for '{currentAuction.Selection}' in progress.");
                validator.Validate(bid > currentAuction.HighBid,
                    $"Bid of '{bid}' is not legal - the current high bid is '{currentAuction.HighBid}'.");
            }

            validator.ValidateMultipleOf(5, bid, $"Bid of '{bid}' is not legal - must be a multiple of 5.");
            validator.Validate(bid <= selection.Value, $"Bid of '{bid}' is not legal - overbidding is not permitted.");
            validator.Validate(round.SeedMoney >= selection.Value - bid, $"Bid of '{bid}' is not legal - not enough seed money.");

            validator.Validate(bid <= actingPlayerState.Money, $"Bid of '{bid}' is not legal - player '{actingPlayerState.Player}' has only {actingPlayerState.Money} cash available.");
        }

        public static GameState MakeBid(GameState gameState, PrivateAuctionRound round, PlayerState biddingPlayerState, PrivateCompany selection, int bid)
        {            
            if (round.CurrentAuction == null)
            {
                // No auction is in progress so start a new auction for the selected private 
                round = round.StartAuction(biddingPlayerState.Player, selection, bid);
            }
            else
            {
                // Apply the new bid to the current auction
                var newAuction = round.CurrentAuction.MakeBid(selection, biddingPlayerState.Player, bid);
                round = round.Update(auction: newAuction, activePlayer: newAuction.GetNextPlayer());
            }

            if (bid == selection.Value)
            {
                // Maximum bid was made, auction terminates                                
                return CompleteAuction(gameState, round);
            }

            return gameState.WithRound(round);
        }

        public static GameState Pass(GameState gameState, PrivateAuctionRound round, Player passingPlayer)
        {                        
            if (round.CurrentAuction != null)
            {
                var nextPlayer = round.CurrentAuction.GetPlayerAfter(passingPlayer);                

                if (nextPlayer == round.CurrentAuction.HighBidder)
                {                    
                    return CompleteAuction(gameState, round);
                }

                // auction continues
                var newAuction = round.CurrentAuction.Pass(passingPlayer);
                round = round.Update(auction: newAuction, activePlayer: newAuction.GetNextPlayer());
                return gameState.WithRound(round);                    
            }            
            else if (round.ActivePlayer == round.LastToAct)
            {
                // Exit the auction round early as everyone passed
                var priorityDeal = round.Players.GetPlayerAfter(round.LastToAct);
                var newRound = new StockRound(1, priorityDeal);
                return new GameState(gameState.Game, newRound, priorityDeal, gameState.PlayerStates, gameState.CompanyStates);
            }
            else
            {
                // player elects not to put anything up for auction
                var newRound = round.Update( activePlayer: gameState.Game.GetPlayerAfter(passingPlayer));
                return gameState.WithRound(newRound);
            }            
        }

        private static GameState CompleteAuction(GameState gameState, PrivateAuctionRound round)
        {
            var currentAuction = round.CurrentAuction;
            var stateForWinningPlayer = gameState.GetPlayerState(currentAuction.HighBidder);
            var winningPlayerPrivates = stateForWinningPlayer.PrivateCompanies.Add(currentAuction.Selection);

            var newPlayerStates = gameState.PlayerStates.Replace(stateForWinningPlayer, 
                new PlayerState(stateForWinningPlayer.Player, stateForWinningPlayer.Money - currentAuction.HighBid, winningPlayerPrivates));                

            var seedFunding = currentAuction.Selection.Value - currentAuction.HighBid;
            var remainingPrivates = round.Privates.Remove(currentAuction.Selection);
            var nextPlayer = round.Players.GetPlayerAfter(round.LastToAct);

            Round newRound;            
            if (remainingPrivates.IsEmpty)
            {
                newRound = new StockRound(1, nextPlayer);
                return new GameState(gameState.Game, newRound, nextPlayer, newPlayerStates, gameState.CompanyStates);
            }
            else
            {
                newRound = new PrivateAuctionRound(round.Players, remainingPrivates, null, nextPlayer, round.LastToAct, round.SeedMoney - seedFunding);
                return new GameState(gameState.Game, newRound, gameState.PlayerWithPriority, newPlayerStates, gameState.CompanyStates);
            }
            
            
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

            public PrivateCompanyAuction MakeBid(PrivateCompany selection, Player player, int bid) =>
                new PrivateCompanyAuction(selection, player, bid, Participants);

            public PrivateCompanyAuction Pass(Player player) =>
                new PrivateCompanyAuction(Selection, HighBidder, HighBid, Participants.Remove(player));

            public Player GetNextPlayer() => Participants.GetPlayerAfter(HighBidder);

        }
    }
}
