using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public abstract class Round
    {
        public int RoundNumber { get; }
        public abstract string Abbreviation { get; }

        public Round(int roundNumber)
        {
            RoundNumber = roundNumber;
        }

        public override string ToString() => $"{Abbreviation}{RoundNumber}";
    }

    public abstract class SubRound : Round
    {        
        public SubRoundMode Mode { get; }

        public SubRound(int roundNumber, SubRoundMode mode) : base(roundNumber)
        {            
            Mode = mode;
        }

        public override string ToString() => $"{Abbreviation}{RoundNumber}{Mode}";

        public enum SubRoundMode { A, B }
    }

    public class StockRound : Round
    {
        public override string Abbreviation { get; } = "SR";

        public StockRound(int roundNumber) : base(roundNumber)
        {

        }
    }

    public class OperatingRound : SubRound
    {
        public override string Abbreviation { get; } = "OR";

        public OperatingRound(int roundNumber, SubRoundMode mode) : base(roundNumber, mode)
        {

        }
    }

    public class MergerRound : SubRound
    {
        public override string Abbreviation { get; } = "MR";

        public MergerRound(int roundNumber, SubRoundMode mode) : base(roundNumber, mode)
        {

        }
    }




}
