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

        public Game(params string[] names)
        {            
            Players = names
                .Select(x => new Player(x))
                .ToImmutableList();            
        }
        
        public Player GetPlayerAfter(Player player) => Players[(Players.IndexOf(player) + 1) % Players.Count];        

        public GameState GetLastValidState(IGameAction finalAction, GameActionValidator validator)
        {
            GameState state = GetInitialState();

            if (finalAction == null)
                return state;

            foreach (var gameAction in finalAction.Sequence)
            {                
                var newState = gameAction.TryApply(state, validator);

                if (validator.IsValid)
                    state = newState;
                else
                {
                    break;
                }
            }

            return state;
        }

        public PendingAction GetPendingAction(GameState gameState)
        {
            var choices = gameState.Round.GetChoices(gameState);
            var activePlayer = gameState.Round.GetActivePlayer(gameState);

            return new PendingAction(activePlayer, choices.ToImmutableList());
        }

        private GameState GetInitialState()
        {
            var startingCash = 1260 / Players.Count;

            var playerStates = Players
                .Select(x => new PlayerState(x, startingCash, hasPriority: Players.IndexOf(x) == 0))
                .ToImmutableList();

            return new GameState(this, PrivateAuctionRound.StartOfAuction(this), playerStates, ImmutableList<CompanyState>.Empty);
        }
        
    }
}
