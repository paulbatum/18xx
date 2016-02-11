﻿using EighteenSeventeen.Core;
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
        public GameStateBuilder Builder { get; } 

        public PrivateAuctionTests()
        {
            Game = new Game("Paul", "Stephen", "Jacky", "Chris");
            Builder = new GameStateBuilder(Game);
        }

        [Fact]
        public void GameHasExpectedStartingConditions()
        {
            var state = Game.GetFinalState();

            Assert.True(state.PlayerStates.All(p => p.Money == 315), "Each player should start with $315 in a 4 player game");
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Paul is the first player so he should have priority");
            Assert.Equal("PR", state.Round.Description);

        }

        [Fact]
        public void GameEntersStockRoundIfAllPlayersPassInPrivateAuction()
        {
            Builder.PlayerPasses("Paul", "Stephen", "Jacky", "Chris");

            var state = Game.GetFinalState();

            Assert.Equal("SR1", state.Round.Description);
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Since everyone passed, Paul should still have priority");
        }

        [Fact]
        public void PlayersCanStartAnAuction()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.CoalMine, 45);            

            var state = Game.GetFinalState();

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

            var state = Game.GetFinalState();

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
        public void PrivateBidsMustBeToTheNearestFiveDollars()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.TrainStation, 63);

            var exception = Assert.Throws<InvalidStepException>(() => Game.GetFinalState());
            Assert.Equal("Bid of '63' is not legal.", exception.Message);
        }

        [Fact]
        public void NextPlayerIsActiveWhenWinningBidIsMade()
        {

        }
    }
}
