using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Actions;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EighteenSeventeen.Test
{    
    public class PrivateAuctionTests
    {
        public Game Game { get; }
        public GameSequenceBuilder Builder { get; }

        public Player Player1 { get; }
        public Player Player2 { get; }
        public Player Player3 { get; }
        public Player Player4 { get; }

        public PrivateAuctionTests()
        {
            Game = new Game("Paul", "Stephen", "Jacky", "Chris");
            Builder = new GameSequenceBuilder(Game);

            Player1 = Game.GetPlayer("Paul");
            Player2 = Game.GetPlayer("Stephen");
            Player3 = Game.GetPlayer("Jacky");
            Player4 = Game.GetPlayer("Chris");
        }

        [Fact]
        public void GameHasExpectedStartingConditions()
        {
            var state = Builder.GetCurrentState();

            Assert.True(state.PlayerStates.All(p => p.Money == 315), "Each player should start with $315 in a 4 player game");
            GameAssert.CurrentRoundIs(state, "PR");            
            GameAssert.PlayerHasPriority(state, Player1);
        }

        [Fact]
        public void StartingPlayerChoosesToBidOnPrivateOrPass()
        {
            var state = Builder.GetCurrentState();
            var pendingAction = Game.GetPendingAction(state);

            var passChoice = pendingAction.Choices.OfType<PassChoice>().SingleOrDefault();
            var bidChoices = pendingAction.Choices.OfType<BidChoice<PrivateCompany>>().ToList();

            Assert.NotNull(passChoice);
            Assert.Equal(PrivateCompanies.All.Count, bidChoices.Count);
        }

        [Fact]
        public void PlayersCanStartAnAuction()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.CoalMine, 45);            

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal(Player2, privateRound.ActivePlayer);

            var auction = privateRound.CurrentAuction;
            Assert.NotNull(auction);            
            Assert.Equal(PrivateCompanies.CoalMine, auction.Selection);
            Assert.Equal(45, auction.HighBid);
            Assert.Equal(Player1, auction.HighBidder);
        }

        [Fact]
        public void WhenAuctionStartsNextPlayerChoosesToBidOrPass()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.CoalMine, 45);
            
            var state = Builder.GetCurrentState();
            var pendingAction = Game.GetPendingAction(state);
            Assert.Equal(Player2, pendingAction.ActivePlayer);

            var passChoice = pendingAction.Choices.OfType<PassChoice>().SingleOrDefault();
            var bidChoice = pendingAction.Choices.OfType<BidChoice<PrivateCompany>>().SingleOrDefault();

            Assert.NotNull(passChoice);
            Assert.NotNull(bidChoice);
            Assert.Equal(PrivateCompanies.CoalMine, bidChoice.Target);
            Assert.Equal(50, bidChoice.Minimum);
            Assert.Equal(60, bidChoice.Maximum);
        }

        [Fact]
        public void PlayersCanPassInsteadOfStartingAnAuction()
        {
            Builder.PlayerPasses(Player1);

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Null(privateRound.CurrentAuction);
            Assert.Equal(Player2, privateRound.ActivePlayer);
        }

        [Fact]
        public void DuringAnAuctionPlayersCanBidOnPrivates()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate(Player2, PrivateCompanies.TrainStation, 70);            

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal(Player3, privateRound.ActivePlayer);

            var auction = privateRound.CurrentAuction;
            Assert.NotNull(auction);
            Assert.Equal(PrivateCompanies.TrainStation, auction.Selection);
            Assert.Equal(70, auction.HighBid);
            Assert.Equal(Player2, auction.HighBidder);
        }

        [Fact]
        public void PlayersThatPassDuringAuctionAreSkippedAfterBid()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.UnionBridge, 55);
            Builder.PlayerPasses(Player2);
            Builder.PlayerBidsOnPrivate(Player3, PrivateCompanies.UnionBridge, 60);
            Builder.PlayerPasses(Player4);

            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.UnionBridge, 65);

            var state = Builder.GetCurrentState();
            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal(Player3, privateRound.ActivePlayer);            
        }

        [Fact]
        public void PlayersThatPassDuringAuctionAreSkippedAfterPass()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.UnionBridge, 55);
            Builder.PlayerPasses(Player2);
            Builder.PlayerBidsOnPrivate(Player3, PrivateCompanies.UnionBridge, 60);
            Builder.PlayerBidsOnPrivate(Player4, PrivateCompanies.UnionBridge, 65);

            Builder.PlayerPasses(Player1);
           
            var state = Builder.GetCurrentState();
            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal(Player3, privateRound.ActivePlayer);
        }

        [Fact]
        public void AuctionCompletesWhenAllOtherPlayersPassOnCurrentBid()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.MajorCoalMine, 65);
            Builder.PlayerBidsOnPrivate(Player2, PrivateCompanies.MajorCoalMine, 70);
            Builder.PlayerPasses(Player3, Player4, Player1);

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Null(privateRound.CurrentAuction);
            Assert.Equal(200 - 20, privateRound.SeedMoney);
            Assert.Equal(Player2, privateRound.ActivePlayer);
            Assert.DoesNotContain(PrivateCompanies.MajorCoalMine, privateRound.Privates);

            var player1State = state.GetPlayerState(Player1);
            Assert.Equal(315, player1State.Money);

            var player2State = state.GetPlayerState(Player2);
            Assert.Equal(315 - 70, player2State.Money);
            Assert.Contains(PrivateCompanies.MajorCoalMine, player2State.PrivateCompanies);
        }


        [Fact]
        public void AuctionCompletesWhenMaximumBidIsMade()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.PittsburghSteelMill, 35);
            Builder.PlayerBidsOnPrivate(Player2, PrivateCompanies.PittsburghSteelMill, 40);

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Null(privateRound.CurrentAuction);
            Assert.Equal(200, privateRound.SeedMoney);
            Assert.Equal(Player2, privateRound.ActivePlayer);

            var player2State = state.GetPlayerState(Player2);
            Assert.Equal(315 - 40, player2State.Money);
            Assert.Contains(PrivateCompanies.PittsburghSteelMill, player2State.PrivateCompanies);
        }

        [Fact]
        public void PrivateBidMustBeHigherThanCurrentHighBid()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate(Player2, PrivateCompanies.TrainStation, 65);
            Builder.AssertValidationErrorForCurrentState("Bid of '65' is not legal - the current high bid is '65'.");
        }

        [Fact]
        public void PrivateBidsMustBeAMultipleOfFive()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 63);
            Builder.AssertValidationErrorForCurrentState("Bid of '63' is not legal - must be a multiple of 5.");            
        }

        [Fact]
        public void PrivateBidCannotExceedFaceValue()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 85);
            Builder.AssertValidationErrorForCurrentState("Bid of '85' is not legal - overbidding is not permitted.");
        }

        [Fact]
        public void PlayersCannotBidOutOfOrder()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate(Player3, PrivateCompanies.TrainStation, 70);
            Builder.AssertValidationErrorForCurrentState("Illegal action - action executed by 'Jacky' but the active player is 'Stephen'.");
        }

        [Fact]
        public void PlayersCannotBidOnDifferentPrivateWhileAuctionIsInProgress()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate(Player2, PrivateCompanies.CoalMine, 45);
            Builder.AssertValidationErrorForCurrentState("Bid on 'Coal Mine' is not legal - there is already an auction for 'Train Station' in progress.");
        }

        [Fact]
        public void PlayersCannotBidMoreThanTheirCurrentCashTotal()
        {
            Builder.PlayerBidsOnPrivate(Player1, PrivateCompanies.TrainStation, 400);
            Builder.AssertValidationErrorForCurrentState("Bid of '400' is not legal - player 'Paul' has only 315 cash available.");
        }       

        //[Fact]
        //public void PrivateBidCannotPutTheSeedMoneyIntoNegative()
        //{
        //    throw new NotImplementedException();
        //}

        //[Fact]
        //public void NextPlayerIsActiveWhenWinningBidIsMade()
        //{
        //    throw new NotImplementedException();
        //}

        [Fact]
        public void GameEntersStockRoundIfAllPlayersPassInPrivateAuction()
        {
            Builder.PlayerPasses(Player1, Player2, Player3, Player4);

            var state = Builder.GetCurrentState();

            GameAssert.CurrentRoundIs(state, "SR1");
            GameAssert.PlayerHasPriority(state, Player1);
        }
    }
}
