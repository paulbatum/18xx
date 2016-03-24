using EighteenSeventeen.Core;
using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using EighteenSeventeen.Core.DataTypes;

namespace EighteenSeventeen.Core
{
    public interface IGameAction
    {
        IGameAction Parent { get; }
        Player ActingPlayer { get; }
        IEnumerable<IGameAction> Sequence { get; }
        GameState TryApply(GameState gameState, GameActionValidator validator);        
    }

    public interface IGameAction<T> : IGameAction where T : Round
    {
        void Validate(GameState gameState, GameActionValidator validator, T round);
        GameState Apply(GameState gameState, T round);
    }

    public abstract class GameAction : IGameAction
    {
        public IGameAction Parent { get; }
        public Player ActingPlayer { get; }
        public abstract bool AppliesToRound(Round round);

        public GameAction(IGameAction parent, Player actingPlayer)
        {
            Parent = parent;
            ActingPlayer = actingPlayer;
        }

        public IEnumerable<IGameAction> Sequence
        {
            get
            {
                if (Parent == null)
                    return new[] { this };

                return Parent.Sequence.Concat(new[] { this });
            }
        }

        public GameState TryApply(GameState gameState, GameActionValidator validator)
        {
            var round = gameState.Round;
            if (AppliesToRound(round) == false)
            {
                validator.Validate(false, $"Game action '{GetType().Name}' does not apply to round of type '{gameState.Round.GetType().Name}'.");
                return null;
            }

            var activePlayer = round.GetActivePlayer(gameState);
            validator.Validate(ActingPlayer == activePlayer, $"Illegal action - action executed by '{ActingPlayer}' but the active player is '{activePlayer}'.");

            // Visitor is for astronauts.
            if (round is PrivateAuctionRound)
                return TryApply<PrivateAuctionRound>(gameState, validator);
            else if (round is StockRound)
                return TryApply<StockRound>(gameState, validator);
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

        public string GetSummary()
        {
            if (IsValid)
                return "There were no validation errors";

            var builder = new StringBuilder();
            builder.AppendLine($"There were {Errors.Count} validation error(s):");

            foreach (var error in Errors)
                builder.AppendLine(error);

            return builder.ToString();
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
