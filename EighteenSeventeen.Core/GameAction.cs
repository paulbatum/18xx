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
    public abstract class GameAction : IGameAction
    {
        public abstract bool AppliesToRound(Round round);

        public void Validate(GameState gameState, GameActionValidator validator)
        {
            validator.Validate(AppliesToRound(gameState.Round), 
                $"Game action '{GetType().Name}' does not apply to round of type '{gameState.Round.GetType().Name}'.");

            var round = gameState.Round;

            // Visitor is for astronauts.
            if (round is PrivateAuctionRound)
                Validate<PrivateAuctionRound>(gameState, validator);
            else if (round is OperatingRound)
                Validate<OperatingRound>(gameState, validator);
            else
                throw new Exception("The type of the current round is not recognized. You probably forgot to update the mess of code above.");
        }

        public GameState Apply(GameState gameState)
        {
            var round = gameState.Round;

            // Visitor is for astronauts.
            if (round is PrivateAuctionRound)
                return Apply<PrivateAuctionRound>(gameState);
            if (round is OperatingRound)
                return Apply<OperatingRound>(gameState);

            throw new Exception("The type of the current round is not recognized. You probably forgot to update the mess of code above.");
        }

        private GameState Apply<T>(GameState gameState) where T : Round
        {
            var self = this as IGameAction<T>;

            if(self == null)
                throw new Exception($"How did we get here - validation should have failed. This action of type '{GetType().Name}' does not match the current round of type '{gameState.Round.GetType().Name}'.");

            return self.Apply(gameState, (T)gameState.Round);
        }

        private void Validate<T>(GameState gameState, GameActionValidator validator) where T : Round
        {
            var self = this as IGameAction<T>;

            if (self == null)
                throw new Exception($"How did we get here - validation should have failed. This action of type '{GetType().Name}' does not match the current round of type '{gameState.Round.GetType().Name}'.");

            self.Validate(gameState, validator, (T)gameState.Round);
        }
    }

    public interface IGameAction
    {        
        void Validate(GameState gameState, GameActionValidator validator);
        GameState Apply(GameState gameState);
    }

    public interface IGameAction<T> : IGameAction where T : Round
    {
        void Validate(GameState gameState, GameActionValidator validator, T round);
        GameState Apply(GameState gameState, T round);
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
