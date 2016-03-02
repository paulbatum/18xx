using EighteenSeventeen.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.DataTypes
{
    // I'm going to regret this
    public abstract class Auction<T>
    {
        public T Selection { get; }        
        public Player HighBidder { get; }
        public int HighBid { get; }
        public ImmutableList<Player> Participants { get; }        

        public Auction(T selection, Player highBidder, int currentBid, ImmutableList<Player> participants)
        {
            Selection = selection;
            HighBidder = highBidder;
            HighBid = currentBid;
            Participants = participants;
        }

        public Player GetPlayerAfter(Player player) => Participants[(Participants.IndexOf(player) + 1) % Participants.Count];
    }
}
