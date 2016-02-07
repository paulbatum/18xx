using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class Train
    {
        public TrainType Type { get; }

        private static ImmutableDictionary<TrainType, int> Costs => new Dictionary<TrainType, int>
        {
            [TrainType.Two] = 100,
            [TrainType.TwoPlus] = 100,
            [TrainType.Three] = 250,
        }.ToImmutableDictionary();

        public Train(TrainType type)
        {
            Type = type;
        }

        public int Cost => Costs[Type];
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
