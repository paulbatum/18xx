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
        public T Target { get; }
        public Player HighBidder { get; }
        public int CurrentBid { get; }

        public Auction(T target, Player highBidder, int currentBid)
        {
            Target = target;
            HighBidder = highBidder;
            CurrentBid = currentBid;
        }
    }
}
