using EighteenSeventeen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EighteenSeventeen.Test
{    
    public class GameStateTests
    {
        [Fact]
        public void PlayersGetStartingMoney()
        {
            var game = new GameStateBuilder("Paul", "Stephen", "Jacky", "Chris")
                .Build();

            var state = game.GetFinalState();

            Assert.True(state.Players.All(p => p.Money == 315), "Each player should start with $315 in a 4 player game");
        }

        [Fact]
        public void GameEntersStockRoundIfAllPlayersPassInPrivateAuction()
        {
            var game = new GameStateBuilder("Paul", "Stephen", "Jacky", "Chris")
                .EachPlayerPasses()
                .Build();

            var state = game.GetFinalState();

            Assert.Equal("SR1", state.Round.Abbreviation);
        }
    }
}
