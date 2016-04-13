using EighteenSeventeen.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Rounds
{
    public class StockRound : PlayerRound
    {
        public const int MaximumIPOBid = 400;
        public int RoundNumber { get; }
        public Auction<Location> CurrentAuction { get; }
        public override string Description => $"SR{RoundNumber}";

        public StockRound(ImmutableList<Player> players, Auction<Location> currentAuction, int roundNumber, Player priority, Player lastToAct) : base(players, priority, lastToAct)
        {
            CurrentAuction = currentAuction;
            RoundNumber = roundNumber;
        }

        public static StockRound StartOfRound(ImmutableList<Player> players, int roundNumber, Player priority) =>
            new StockRound(players, null, roundNumber, priority, null);

        private StockRound Update(Auction<Location> auction = null, Player activePlayer = null, Player lastToAct = null) =>
            new StockRound(Players, auction ?? CurrentAuction, RoundNumber, activePlayer ?? ActivePlayer, lastToAct ?? LastToAct);

        private StockRound StartAuction(Player biddingPlayer, Location selection, int bid)
        {
            var auction = new Auction<Location>(selection, biddingPlayer, bid, Players);
            return Update(auction: auction, activePlayer: Players.GetPlayerAfter(biddingPlayer), lastToAct: biddingPlayer);
        }

        public static void ValidateIPOBid(GameState state, GameActionValidator validator, StockRound round, PlayerState actingPlayerState, Location selection, int bid)
        {
            var currentAuction = round.CurrentAuction;
            if (currentAuction != null)
            {
                validator.Validate(selection == currentAuction.Selection,
                    $"Bid on '{selection}' is not legal - there is already an auction for '{currentAuction.Selection}' in progress.");
                validator.Validate(bid > currentAuction.HighBid,
                    $"Bid of '{bid}' is not legal - the current high bid is '{currentAuction.HighBid}'.");
            }

            validator.ValidateMultipleOf(5, bid, $"Bid of '{bid}' is not legal - must be a multiple of 5.");
            validator.Validate(bid <= MaximumIPOBid, $"Bid of '{bid}' is not legal - the maximum IPO bid is $400.");            
            validator.Validate(bid <= actingPlayerState.Money, $"Bid of '{bid}' is not legal - player '{actingPlayerState.Player}' has only {actingPlayerState.Money} cash available.");
        }

        public static GameState MakeIPOBid(GameState gameState, StockRound round, PlayerState biddingPlayerState, Location selection, int bid)
        {
            if (round.CurrentAuction == null)
            {
                // No auction is in progress so start a new auction for the selected private 
                round = round.StartAuction(biddingPlayerState.Player, selection, bid);
            }
            else
            {
                // Apply the new bid to the current auction
                var newAuction = round.CurrentAuction.MakeBid(selection, biddingPlayerState.Player, bid);
                round = round.Update(auction: newAuction, activePlayer: newAuction.GetNextPlayer());
            }

            if (bid == MaximumIPOBid)
            {
                // Maximum bid was made, auction terminates                                
                return CompleteAuction(gameState, round);
            }

            return gameState.WithRound(round);
        }

        private static GameState CompleteAuction(GameState gameState, StockRound round)
        {
            // Go into a state that requires the player to specify funding
            throw new NotImplementedException();
        }

        public override IEnumerable<IChoice> GetChoices(GameState gameState)
        {
            throw new NotImplementedException();
        }


    }
}
