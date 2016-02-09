using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.GameSteps
{
    public class PlayerPassStep : GameStep
    {
        public Player ActingPlayer { get; }

        public PlayerPassStep(Player actingPlayer)
        {
            ActingPlayer = actingPlayer;
        }

        public override GameState Apply(GameState currentState)
        {
            var currentRound = currentState.Round as PlayerRound;

            if (currentRound == null)
                throw new ArgumentException("A pass step is only valid during a player round");

            if (ActingPlayer != currentRound.ActivePlayer)
                throw new InvalidStepException(this, currentState, $"Expected step from player '{currentRound.ActivePlayer}. Instead got step from player '{ActingPlayer}'.");

            Round newRound;

            if (currentRound.ActivePlayer == currentRound.LastToAct)
            {
                newRound = currentRound.NextRound(currentState);
            }
            else
            {
                newRound = currentRound.AdvanceToNextPlayer(currentState);
            }

            return new GameState(currentState.Game, newRound, currentState.PlayerStates, currentState.CompanyStates);
        }
    }
}
