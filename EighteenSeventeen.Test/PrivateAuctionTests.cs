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

        public PrivateAuctionTests()
        {
            Game = new Game("Paul", "Stephen", "Jacky", "Chris");
            Builder = new GameSequenceBuilder(Game);
        }

        [Fact]
        public void GameHasExpectedStartingConditions()
        {
            var state = Builder.GetCurrentState();

            Assert.True(state.PlayerStates.All(p => p.Money == 315), "Each player should start with $315 in a 4 player game");
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Paul is the first player so he should have priority");
            Assert.Equal("PR", state.Round.Description);

        }

        //[Fact]
        //public void PlayerWithPriorityCanBidOnPrivateOrPass()
        //{
        //    var pendingAction = Game.GetPendingAction();

        //    var passChoice = pendingAction.Choices.OfType<PlayerPassAction>().FirstOrDefault();
        //    var bidChoice = pendingAction.Choices.OfType<PlayerPrivateBidAction>().FirstOrDefault();

        //    Assert.NotNull(passChoice);
        //    Assert.NotNull(bidChoice);
        //}

        //[Fact]
        //public void PlayerWithPriorityCanSelectWhichPrivateToBidOn()
        //{
        //    var pendingAction = Game.GetPendingAction();

        //    var bidChoice = pendingAction.Choices.FirstOrDefaultOfType<PlayerPrivateBidAction>();
        //    Assert.NotNull(bidChoice);


        //}


        [Fact]
        public void GameEntersStockRoundIfAllPlayersPassInPrivateAuction()
        {
            Builder.PlayerPasses("Paul", "Stephen", "Jacky", "Chris");

            var state = Builder.GetCurrentState();

            Assert.Equal("SR1", state.Round.Description);
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Since everyone passed, Paul should still have priority");
        }

        [Fact]
        public void PlayersCanStartAnAuction()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.CoalMine, 45);            

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal("Stephen", privateRound.ActivePlayer.Name);

            var auction = privateRound.CurrentAuction;
            Assert.NotNull(auction);            
            Assert.Equal(PrivateCompanies.CoalMine, auction.Selection);
            Assert.Equal(45, auction.CurrentBid);
            Assert.Equal("Paul", auction.HighBidder.Name);
        }

        [Fact]
        public void PlayersCanBidOnPrivates()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate("Stephen", PrivateCompanies.TrainStation, 70);            

            var state = Builder.GetCurrentState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal("Jacky", privateRound.ActivePlayer.Name);

            var auction = privateRound.CurrentAuction;
            Assert.NotNull(auction);
            Assert.Equal(PrivateCompanies.TrainStation, auction.Selection);
            Assert.Equal(70, auction.CurrentBid);
            Assert.Equal("Stephen", auction.HighBidder.Name);
        }

        [Fact]
        public void PrivateBidMustBeHigherThanCurrentHighBid()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate("Stephen", PrivateCompanies.TrainStation, 65);
            Builder.AssertValidationErrorForCurrentState("Bid of '65' is not legal - the current high bid is '65'.");
        }

        [Fact]
        public void PrivateBidsMustBeAMultipleOfFive()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 63);
            Builder.AssertValidationErrorForCurrentState("Bid of '63' is not legal - must be a multiple of 5.");            
        }

        [Fact]
        public void PrivateBidCannotExceedFaceValue()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 85);
            Builder.AssertValidationErrorForCurrentState("Bid of '85' is not legal - overbidding is not permitted.");
        }

        [Fact]
        public void PlayersCannotBidOutOfOrder()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate("Jacky", PrivateCompanies.TrainStation, 70);
            Builder.AssertValidationErrorForCurrentState("Illegal action - action executed by 'Jacky' but the active player is 'Stephen'.");
        }

        [Fact]
        public void PlayersCannotBidOnDifferentPrivateWhileAuctionIsInProgress()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 65);
            Builder.PlayerBidsOnPrivate("Stephen", PrivateCompanies.CoalMine, 45);
            Builder.AssertValidationErrorForCurrentState("Bid on 'Coal Mine' is not legal - there is already an auction for 'Train Station' in progress.");
        }

        [Fact]
        public void PlayersCannotBidMoreThanTheirCurrentCashTotal()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 400);
            Builder.AssertValidationErrorForCurrentState("Bid of '400' is not legal - player 'Paul' has only 315 cash available.");
        }

        [Fact]
        public void PrivateBidCannotPutTheSeedMoneyIntoNegative()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void NextPlayerIsActiveWhenWinningBidIsMade()
        {
            throw new NotImplementedException();
        }
    }
}
