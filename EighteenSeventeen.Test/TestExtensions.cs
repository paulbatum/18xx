using EighteenSeventeen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Test
{
    public static class TestExtensions
    {
        public static Player GetPlayer(this Game game, string name)
        {
            var player = game.Players.SingleOrDefault(p => p.Name == name);

            if (player == null)
                throw new ArgumentException($"Player '{name}' not found.");

            return player;
        }

        public static PlayerState GetPlayerState(this GameState state, string name)
        {
            var player = state.PlayerStates.SingleOrDefault(p => p.Player.Name == name);

            if (player == null)
                throw new ArgumentException($"Player '{name}' not found.");

            return player;
        }
    }
}
