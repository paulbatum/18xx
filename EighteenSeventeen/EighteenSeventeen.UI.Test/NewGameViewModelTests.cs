using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Xunit;

namespace EighteenSeventeen.UI.Test
{    
    public class NewGameViewModelTests
    {
        private NewGameViewModel viewmodel;

        public NewGameViewModelTests()
        {
            viewmodel = new NewGameViewModel();              
        }

        [Fact]
        public void CanAddUpToSevenPlayers()
        {            
            foreach(var i in Enumerable.Range(1, 7))
            {
                viewmodel.NewPlayerName = "Player" + i;                
                Assert.True(viewmodel.AddPlayer.CanExecute(null));
                
                viewmodel.AddPlayer.Execute(null);
                Assert.Equal(i, viewmodel.Players.Count);
            }

            viewmodel.NewPlayerName = "Player8";   
            Assert.False(viewmodel.AddPlayer.CanExecute(null));
        }

        [Fact]
        public void CannotAddPlayerWithNoName()
        {
            viewmodel.NewPlayerName = string.Empty;
            Assert.False(viewmodel.AddPlayer.CanExecute(null));
        }

        [Fact]
        public void CannotAddDuplicateName()
        {
            var playerName = "player1";
            viewmodel.NewPlayerName = playerName;            
            Assert.True(viewmodel.AddPlayer.CanExecute(null));

            viewmodel.AddPlayer.Execute(null);
            viewmodel.NewPlayerName = playerName;
            Assert.False(viewmodel.AddPlayer.CanExecute(null));
        }

        [Fact]
        public void CanStartGameWithThreeOrMorePlayers()
        {            
            foreach (var i in Enumerable.Range(1, 7))
            {
                viewmodel.NewPlayerName = "Player" + i;
                viewmodel.AddPlayer.Execute(null);
                                
                Assert.Equal(i >= 3, viewmodel.StartGame.CanExecute(null));
            }            
        }
    }
}
