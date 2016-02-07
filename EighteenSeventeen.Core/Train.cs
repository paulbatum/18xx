using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class Train
    {
        public TrainType Type { get; }

        public Train(TrainType type)
        {
            Type = type;
        }
    }

    public enum TrainType
    {
        Two,
        TwoPlus,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight
    }
}
