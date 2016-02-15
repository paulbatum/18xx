using EighteenSeventeen.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core.Actions
{
    public class PlayerPassAction : GameAction, IGameAction<PrivateAuctionRound>
    {
        public Player ActingPlayer { get; }

        public PlayerPassAction(Player actingPlayer)
        {
            ActingPlayer = actingPlayer;
        }

        public void Validate(GameState gameState, GameActionValidator validator, PrivateAuctionRound round)
        {
            // I need to refactor this
            validator.Validate(round.ActivePlayer == ActingPlayer, "players dont match");
        }

        public GameState Apply(GameState gameState, PrivateAuctionRound round)
        {
            return round.Pass(gameState, this.ActingPlayer);
        }

        public override bool AppliesToRound(Round round)
        {
            return round is PrivateAuctionRound;
        }

        //public override GameActionValidationResult Validate(GameState gameState)
        //{
        //    throw new NotImplementedException();
        //}

        //public override GameState Apply(GameState currentState)
        //{
        //    var currentRound = currentState.Round as PlayerRound;

        //    if (currentRound == null)
        //        throw new ArgumentException("A pass step is only valid during a player round");

        //    if (ActingPlayer != currentRound.ActivePlayer)
        //        throw new InvalidActionException(this, currentState, $"Expected step from player '{currentRound.ActivePlayer}. Instead got step from player '{ActingPlayer}'.");

        //    Round newRound;

        //    if (currentRound.ActivePlayer == currentRound.LastToAct)
        //    {
        //        newRound = currentRound.NextRound(currentState);
        //    }
        //    else
        //    {
        //        newRound = currentRound.AdvanceToNextPlayer(currentState);
        //    }

        //    return new GameState(currentState.Game, newRound, currentState.PlayerStates, currentState.CompanyStates);
        //}
    }
}
