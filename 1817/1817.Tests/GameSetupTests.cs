using _1817.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace _1817.Tests
{
    public class GameSetupTests
    {        
        [Theory]        
        [InlineDataAttribute(3, 420)]
        [InlineDataAttribute(4, 315)]
        [InlineDataAttribute(5, 252)]
        [InlineDataAttribute(6, 210)]
        [InlineDataAttribute(7, 180)]
        public void PlayersHaveCorrectStartingMoney(int numberOfPlayers, int expectedCash)
        {
            var game = GameSetup.GameWithNPlayers(numberOfPlayers);

            Assert.Equal(numberOfPlayers, game.Players.Count);
            foreach (var p in game.Players)
            {
                Assert.Equal(expectedCash, p.Money);
            }
        }

        [Fact]
        public void StartingAuctionHasPrivatesAndSeedMoney()
        {
            var game = GameSetup.GameWithNPlayers(3);
            Assert.Equal(11, game.OpeningAuction.Privates.Count);
            Assert.Equal(200, game.OpeningAuction.SeedMoney.Money);
        }
    }

    public static class GameSetup
    {
        public static Game GameWithNPlayers(int numberOfPlayers)
        {
            var playerNames = Enumerable.Range(1, numberOfPlayers).Select(i => "Player" + i).ToList();
            return new Game(playerNames);
        }
    }
}
