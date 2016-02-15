using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Actions;
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

        public GameStateBuilder(Game game)
        {
            Game = game;
        }

        public void PlayerPasses(params string[] playerName)
        {
            var steps = playerName.Select(p => new PlayerPassAction(Game.GetPlayer(p)));
            Game.GameSequence.AddRange(steps);
        }

        public void PlayerBidsOnPrivate(string playerName, PrivateCompany privateCompany, int bid)
        {
            var step = new PlayerPrivateBidAction(Game.GetPlayer(playerName), privateCompany, bid);
            Game.GameSequence.Add(step);
        }
    }
}
