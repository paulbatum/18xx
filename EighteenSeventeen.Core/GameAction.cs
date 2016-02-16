using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace EighteenSeventeen.Core
{
    public interface IGameAction
    {
        GameState TryApply(GameState gameState, GameActionValidator validator);
    }

    public interface IGameAction<T> : IGameAction where T : Round
    {
        void Validate(GameState gameState, GameActionValidator validator, T round);
        GameState Apply(GameState gameState, T round);
    }

    public abstract class GameAction : IGameAction
    {
        public abstract bool AppliesToRound(Round round);

        public GameState TryApply(GameState gameState, GameActionValidator validator)
        {
            var round = gameState.Round;
            if (AppliesToRound(round) == false)
            {
                validator.Validate(false, $"Game action '{GetType().Name}' does not apply to round of type '{gameState.Round.GetType().Name}'.");
                return null;
            }            

            // Visitor is for astronauts.
            if (round is PrivateAuctionRound)
                return TryApply<PrivateAuctionRound>(gameState, validator);
            else if (round is OperatingRound)
                return TryApply<OperatingRound>(gameState, validator);
            else
                throw new Exception($"Current round of type '{round.GetType().Name}' is not recognized. You probably forgot to update the mess of code above.");
        }       

        private GameState TryApply<T>(GameState gameState, GameActionValidator validator) where T : Round
        {
            var self = this as IGameAction<T>;
            if (self == null)
                throw new Exception($"How did we get here - validation should have failed. This action of type '{GetType().Name}' does not match the current round of type '{gameState.Round.GetType().Name}'.");

            var round = (T)gameState.Round;
            self.Validate(gameState, validator, round);

            if (validator.IsValid)
                return self.Apply(gameState, round);
            else
                return null;
        }
    }

    public class GameActionValidator
    {        
        public List<string> Errors { get; }
        public bool IsValid => Errors.Count == 0;
        
        public GameActionValidator()
        {
            Errors = new List<string>();
        }

        public void Validate(bool condition, string errorMessage)
        {
            if (condition == false)
                Errors.Add(errorMessage);
        }

        public void ValidateMultipleOf(int multiple, int value, string message)
        {
            if (value % multiple != 0)
                Errors.Add(message);
        }
    }
}
