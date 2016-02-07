using EighteenSeventeen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Test
{
    public class GameStateBuilder
    {
        private Game Game { get; }

        public GameStateBuilder(params string[] playerNames)
        {
            Game = new Game(playerNames);
        }        

        public static GameStateBuilder NewDefaultGame()
        {            
            return new GameStateBuilder("Paul", "Stephen", "Jacky");
        }

        public Game Build()
        {
            return Game;
        }

        public GameStateBuilder EachPlayerPasses()
        {
            return this;
        }
    }
}
