using EighteenSeventeen.Core;
using EighteenSeventeen.Core.DataTypes;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EighteenSeventeen.Test
{
    public class StockRoundTests
    {
        public Game Game { get; }
        public GameSequenceBuilder Builder { get; }

        public Player Player1 { get; }
        public Player Player2 { get; }
        public Player Player3 { get; }
        public Player Player4 { get; }

        public StockRoundTests()
        {
            Game = new Game("Paul", "Stephen", "Jacky", "Chris");
            Builder = new GameSequenceBuilder(Game);

            Player1 = Game.GetPlayer("Paul");
            Player2 = Game.GetPlayer("Stephen");
            Player3 = Game.GetPlayer("Jacky");
            Player4 = Game.GetPlayer("Chris");
        }

        [Fact]
        public void PlayersCanBidToStartCompanies()
        {            
            Builder.PlayerPasses(Player1, Player2, Player3, Player4);

            // Start of SR
            Builder.PlayerStartsAnIPO(Player1, Locations.SouthNewYork, 100);

            var state = Builder.GetCurrentState();
            var stockRound = state.Round as StockRound;
            Assert.NotNull(stockRound);

            var auction = stockRound.CurrentAuction;
            Assert.NotNull(auction);
            Assert.Equal(Locations.SouthNewYork, auction.Selection);
            Assert.Equal(100, auction.HighBid);
            Assert.Equal(Player1, auction.HighBidder);
        }
    }
}
