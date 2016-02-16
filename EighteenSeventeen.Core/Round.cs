using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EighteenSeventeen.Core.Actions;

namespace EighteenSeventeen.Core
{
    public abstract class Round
    {        
        public abstract string Description { get; }
        public abstract Player GetActivePlayer(GameState gameState);
        public abstract Round NextRound(GameState gameState);

        //public abstract GameActionValidationResult ValidateGameAction(GameState gameState, GameAction gameAction);
        //public abstract GameState ApplyGameAction(GameState gameState, GameAction gameAction);
        
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

        public override Player GetActivePlayer(GameState gameState)
        {
            return ActivePlayer;
        }
    }

    public abstract class CompanyRound : Round
    {
        public Company ActiveCompany { get; }

        public CompanyRound(Company activeCompany)
        {
            ActiveCompany = activeCompany;
        }

        public override Player GetActivePlayer(GameState gameState)
        {
            return gameState.GetOwner(ActiveCompany);
        }
    }

    public enum RoundMode { A, B }

    

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
