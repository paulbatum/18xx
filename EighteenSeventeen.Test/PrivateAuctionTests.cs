using EighteenSeventeen.Core;
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
            Builder.EachPlayerPasses();            

            var state = Game.GetFinalState();

            Assert.Equal("SR1", state.Round.Description);
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Since everyone passed, Paul should still have priority");
        }

        [Fact]
        public void PlayersCanBidOnPrivates()
        {
            Builder.PlayerBidsOnPrivate("Paul", PrivateCompanies.CoalMine, 45);            

            var state = Game.GetFinalState();

            var privateRound = state.Round as PrivateAuctionRound;
            Assert.NotNull(privateRound);
            Assert.Equal("Stephen", privateRound.ActivePlayer.Name);

            var auction = privateRound.CurrentAuction;
            Assert.NotNull(auction);            
            Assert.Equal(PrivateCompanies.CoalMine, auction.Target);
            Assert.Equal(45, auction.CurrentBid);
            Assert.Equal("Paul", auction.HighBidder.Name);
        }
    }
}
