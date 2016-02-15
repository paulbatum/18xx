using EighteenSeventeen.Core.Rounds;
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
        public List<IGameAction> GameSequence { get; } = new List<IGameAction>();

        public Game(params string[] names)
        {            
            Players = names
                .Select(x => new Player(x))
                .ToImmutableList();            
        }

        
        public Player GetPlayerAfter(Player player) => Players[(Players.IndexOf(player) + 1) % Players.Count];        

        public PendingAction GetPendingAction()
        {
            var state = GetCurrentState();
            return null;
        }

        public GameState GetCurrentState()
        {
            GameState state = GetInitialState();

            foreach (var gameAction in GameSequence)
            {
                var validator = new GameActionValidator();
                gameAction.Validate(state, validator);

                if (validator.IsValid)
                    state = gameAction.Apply(state);
                else
                {
                    // todo, handle this properly
                    throw new Exception("There were game action validation errors");
                }
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
