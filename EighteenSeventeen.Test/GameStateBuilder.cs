using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Test
{
    public class GameSequenceBuilder
    {
        public Game Game { get; }
        public IGameAction LastAction { get; private set; }

        public GameSequenceBuilder(Game game)
        {
            Game = game;
        }

        public IGameAction PlayerPasses(params string[] playerNames)
        {
            foreach (var name in playerNames)
                LastAction = new PlayerPassAction(LastAction, Game.GetPlayer(name));

            return LastAction;                
        }

        public IGameAction PlayerBidsOnPrivate(string playerName, PrivateCompany privateCompany, int bid)
        {
            LastAction = new PlayerPrivateBidAction(LastAction, Game.GetPlayer(playerName), privateCompany, bid);
            return LastAction;
        }

        public GameState GetCurrentState()
        {
            var validator = new GameActionValidator();
            var state = Game.GetLastValidState(LastAction, validator);

            if (validator.IsValid == false)
                throw new Exception(validator.GetSummary());

            return state;
        }
    }
}
