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
        public abstract GameState Apply(GameState currentState);        
    }

    public class InvalidStepException : Exception
    {
        public GameStep Step { get; }
        public GameState GameState { get; }
                
        public InvalidStepException(GameStep step, GameState gameState, string message) : base(message)
        {
            Step = step;
            GameState = gameState;
        }
    }    

}
