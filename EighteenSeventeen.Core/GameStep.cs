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
        public Game Game { get; }
        public abstract GameState Apply(GameState current);
    }


}
