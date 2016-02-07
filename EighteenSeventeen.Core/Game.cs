using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class Game
    {
        public IImmutableList<Player> Players { get; }
        public List<GameStep> Steps { get; } = new List<GameStep>();

        public Game(params string[] names)
        {            
            Players = names
                .Select(x => new Player(x))
                .ToImmutableList();            
        }

        public GameState GetFinalState()
        {
            var state = GetInitialState();

            foreach (var step in Steps)
            {
                state = step.Apply(state);
            }

            return state;
        }

        private GameState GetInitialState()
        {
            var startingCash = 1260 / Players.Count;

            var playerStates = Players
                .Select(x => new PlayerState(x, startingCash))
                .ToImmutableList();

            return new GameState(this, new PrivateAuctionRound(), playerStates, ImmutableList<CompanyState>.Empty);
        }
        
    }
    
    public class GameSequence
    {
        
    }

    public class GameState
    {
        public Game Game { get; }
        public Round Round { get; }
        public ImmutableList<PlayerState> Players { get; }
        public ImmutableList<CompanyState> Companies { get; }        

        public GameState(Game game, Round round, ImmutableList<PlayerState> playerStates, ImmutableList<CompanyState> companyStates)
        {
            Game = game;
            Round = round;
            Players = playerStates;
            Companies = companyStates;
        }

    }
}
