using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public abstract class Round
    {
        public abstract string Description { get; }
        public abstract Round NextRound(GameState gameState);        
    }    

    public abstract class PlayerRound : Round
    {
        public Player ActivePlayer { get; }
        public Player LastToAct { get; }
        public abstract Round AdvanceToNextPlayer(GameState gameState);

        public PlayerRound(Player activePlayer, Player lastToAct)
        {
            ActivePlayer = activePlayer;
            LastToAct = lastToAct;
        }        
    }

    public abstract class CompanyRound : Round
    {
        public Company ActiveCompany { get; }

        public CompanyRound(Company activeCompany)
        {
            ActiveCompany = activeCompany;
        }
    }

    public enum RoundMode { A, B }

    public class PrivateAuctionRound : PlayerRound
    {
        public override string Description { get; } = "PR";
        public ImmutableList<PrivateCompany> Privates { get; }
        public Auction<PrivateCompany> CurrentAuction { get; }

        public PrivateAuctionRound(ImmutableList<PrivateCompany> privates, Auction<PrivateCompany> auction, Player activePlayer, Player lastToAct) 
            : base(activePlayer, lastToAct)
        {
            Privates = privates;
            CurrentAuction = auction;
        }

        public static PrivateAuctionRound StartOfAuction(Game game)
        {            
            return new PrivateAuctionRound(PrivateCompanies.All, null, game.Players.First(), game.Players.Last());
        }        

        public override Round AdvanceToNextPlayer(GameState state)
        {
            return new PrivateAuctionRound(Privates, CurrentAuction, state.Game.GetPlayerAfter(this.ActivePlayer), LastToAct);
        }

        public override Round NextRound(GameState gameState)
        {
            var priorityDeal = gameState.Game.GetPlayerAfter(LastToAct);
            return new StockRound(1, priorityDeal);
        }

        public PrivateAuctionRound Bid(GameState gameState, Player player, PrivateCompany target, int bid)
        {
            var auction = new Auction<PrivateCompany>(target, player, bid);
            return new PrivateAuctionRound(Privates, auction, gameState.Game.GetPlayerAfter(player), player);
        }
    }

    public class StockRound : PlayerRound
    {
        public int RoundNumber { get; }
        public override string Description => $"SR{RoundNumber}";        

        public StockRound(int roundNumber, Player priority) : base(priority, null)
        {
            RoundNumber = roundNumber;
        }

        public override Round NextRound(GameState gameState)
        {
            throw new NotImplementedException();
        }

        public override Round AdvanceToNextPlayer(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }

    public class OperatingRound : CompanyRound
    {
        public int RoundNumber { get; }
        public RoundMode RoundMode { get; }
        public override string Description => $"OR{RoundNumber}{RoundMode}";

        public OperatingRound(GameState gameState, Company activeCompany, int roundNumber, RoundMode mode) : base(activeCompany)
        {
            RoundNumber = roundNumber;
            RoundMode = mode;
        }

        public override Round NextRound(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }

    public class MergerRound : CompanyRound
    {
        public int RoundNumber { get; }
        public RoundMode RoundMode { get; }
        public override string Description => $"MR{RoundNumber}{RoundMode}";

        public MergerRound(Company activeCompany, int roundNumber, RoundMode mode) : base(activeCompany)
        {
            RoundNumber = roundNumber;
            RoundMode = mode;
        }

        public override Round NextRound(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }    
}
