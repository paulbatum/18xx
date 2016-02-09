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

        public Player GetPlayerAfter(Player player) => Players[(Players.IndexOf(player) + 1) % Players.Count];        

        public GameState GetFinalState()
        {
            GameState state = GetInitialState();

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
                .Select(x => new PlayerState(x, startingCash, hasPriority: Players.IndexOf(x) == 0))
                .ToImmutableList();

            return new GameState(this,PrivateAuctionRound.StartOfAuction(this), playerStates, ImmutableList<CompanyState>.Empty);
        }
        
    }
}
