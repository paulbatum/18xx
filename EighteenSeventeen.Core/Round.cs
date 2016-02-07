using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public abstract class Round
    {
        public abstract string Abbreviation { get; }

        public override string ToString() => $"{Abbreviation}";
    }

    public abstract class NumberedRound : Round
    {
        public int RoundNumber { get; }        

        public NumberedRound(int roundNumber)
        {
            RoundNumber = roundNumber;
        }

        public override string ToString() => $"{Abbreviation}{RoundNumber}";
    }

    public abstract class SubRound : NumberedRound
    {        
        public SubRoundMode Mode { get; }

        public SubRound(int roundNumber, SubRoundMode mode) : base(roundNumber)
        {            
            Mode = mode;
        }

        public override string ToString() => $"{Abbreviation}{RoundNumber}{Mode}";

        public enum SubRoundMode { A, B }
    }

    public class PrivateAuctionRound : Round
    {
        public override string Abbreviation { get; } = "PR";
    }

    public class StockRound : NumberedRound
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
