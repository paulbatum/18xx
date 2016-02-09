using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public abstract class GameStep
    {        
        public abstract GameState Apply(GameState current);        
    }

    public class PlayerPassStep : GameStep
    {        
        public override GameState Apply(GameState currentState)
        {
            var currentRound = currentState.Round as PlayerRound;
                     
            if (currentRound == null)
                throw new ArgumentException("A pass step is only valid during a player round");

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
