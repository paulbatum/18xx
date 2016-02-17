using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public interface IChoice
    {
        string Description { get; }
    }

    public class PassChoice : IChoice
    {
        public string Description => "Pass";
        public override string ToString() => Description;
    }

    public class BidChoice<T> : IChoice
    {
        public T Target { get; }
        public int Minimum { get; }
        public int Maximum { get; }

        public BidChoice(T target, int min, int max)
        {
            Target = target;
            Minimum = min;
            Maximum = max;
        }

        public string Description => $"{Target}: {Minimum} - {Maximum}";
        public override string ToString() => Description;
    }
}
