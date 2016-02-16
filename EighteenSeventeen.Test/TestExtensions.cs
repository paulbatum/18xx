using EighteenSeventeen.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

        public static T FirstOrDefaultOfType<T>(this IEnumerable enumerable)
        {
            return enumerable.OfType<T>().FirstOrDefault();
        }

        public static void AssertValidationErrorForCurrentState(this GameSequenceBuilder builder, string message)
        {
            var validator = new GameActionValidator();
            var state = builder.Game.GetLastValidState(builder.LastAction, validator);

            if (validator.IsValid)
                throw new Exception("Validator reports valid, expected error: " + message);

            Assert.Contains(validator.Errors, e => e == message);            
        }
    }
}
