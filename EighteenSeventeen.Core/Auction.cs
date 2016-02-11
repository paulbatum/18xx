using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    // I'm going to regret this
    public class Auction<T>
    {
        public T Selection { get; }
        public Player HighBidder { get; }
        public int CurrentBid { get; }

        public Auction(T selection, Player highBidder, int currentBid)
        {
            Selection = selection;
            HighBidder = highBidder;
            CurrentBid = currentBid;
        }
    }
}
