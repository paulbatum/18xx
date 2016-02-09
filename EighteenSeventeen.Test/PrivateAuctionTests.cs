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
        [Fact]
        public void GameHasExpectedStartingConditions()
        {
            var game = new GameStateBuilder("Paul", "Stephen", "Jacky", "Chris")
                .Build();

            var state = game.GetFinalState();

            Assert.True(state.PlayerStates.All(p => p.Money == 315), "Each player should start with $315 in a 4 player game");
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Paul is the first player so he should have priority");
            Assert.Equal("PR", state.Round.Description);

        }

        [Fact]
        public void GameEntersStockRoundIfAllPlayersPassInPrivateAuction()
        {
            var game = new GameStateBuilder("Paul", "Stephen", "Jacky", "Chris")
                .EachPlayerPasses()
                .Build();

            var state = game.GetFinalState();

            Assert.Equal("SR1", state.Round.Description);
            Assert.True(state.GetPlayerState("Paul").HasPriority, "Since everyone passed, Paul should still have priority");
        }
    }
}
