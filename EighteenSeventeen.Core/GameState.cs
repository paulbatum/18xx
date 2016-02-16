using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Core
{
    public class GameState
    {
        public Game Game { get; }
        public Round Round { get; }
        public ImmutableList<PlayerState> PlayerStates { get; }
        public ImmutableList<CompanyState> CompanyStates { get; }
        

        public GameState(Game game, Round round, ImmutableList<PlayerState> playerStates, ImmutableList<CompanyState> companyStates)
        {
            Game = game;
            Round = round;            
            PlayerStates = playerStates;
            CompanyStates = companyStates;
        }

        public PlayerState GetPlayerState(Player player) => PlayerStates.Single(s => s.Player == player);
        public CompanyState GetCompanyState(Company company) => CompanyStates.Single(s => s.Company == company);
        public Player GetOwner(Company company)
        {
            throw new NotImplementedException();
        }

    }
}
