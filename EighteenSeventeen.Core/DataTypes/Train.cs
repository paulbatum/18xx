﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace EighteenSeventeen.Core.DataTypes
{
    public class Train
    {
        public TrainType Type { get; }

        private readonly static ImmutableDictionary<TrainType, int> Costs = new Dictionary<TrainType, int>
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
