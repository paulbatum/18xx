using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Actions;
using EighteenSeventeen.Core.DataTypes;
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

        public IGameAction PlayerPasses(params Player[] players)
        {
            foreach (var player in players)
                LastAction = new PlayerPassAction(LastAction, player);

            return LastAction;                
        }

        public IGameAction PlayerBidsOnPrivate(Player player, PrivateCompany privateCompany, int bid)
        {
            LastAction = new PlayerPrivateBidAction(LastAction, player, privateCompany, bid);
            return LastAction;
        }


        public void PlayerStartsAnIPO(Player player, Location location, int bid)
        {
            LastAction = new PlayerIPOBidAction(LastAction, player, location, bid);            
        }

        public GameState GetCurrentState()
        {
            var validator = new GameActionValidator();
            var state = Game.GetLastValidState(LastAction, validator);

            if (validator.IsValid == false)
                throw new Exception(validator.GetSummary());

            return state;
        }

        public PendingAction GetCurrentPendingAction()
        {
            var state = GetCurrentState();
            return Game.GetPendingAction(state);
        }
    }
}
