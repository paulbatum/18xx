using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.GameSteps
{
    public class PlayerPrivateBidStep : GameStep
    {
        public Player Player { get; }
        public PrivateCompany Target { get; }        
        public int Bid { get; }

        public PlayerPrivateBidStep(Player player, PrivateCompany target, int bid)
        {
            Player = player;
            Target = target;
            Bid = bid;
        }

        public override GameState Apply(GameState currentState)
        {
            var currentRound = currentState.Round as PrivateAuctionRound;

            if (currentRound == null)
                throw new ArgumentException("A private bid step is only valid during the private auction round");

            currentRound = currentRound.Bid(currentState, Player, Target, Bid);
            return new GameState(currentState.Game, currentRound, currentState.PlayerStates, currentState.CompanyStates);            
        }
    }
}
