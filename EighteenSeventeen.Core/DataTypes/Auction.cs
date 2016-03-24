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
    public class Auction<T>
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

        public Player GetPlayerAfter(Player player) => Participants.GetPlayerAfter(player);
        public Player GetNextPlayer() => GetPlayerAfter(HighBidder);
        public bool IsComplete => Participants.Count == 1;

        public Auction<T> MakeBid(T selection, Player player, int bid) => new Auction<T>(selection, player, bid, Participants);
        public Auction<T> Pass(Player player) => new Auction<T>(Selection, HighBidder, HighBid, Participants.Remove(player));
    }
}
