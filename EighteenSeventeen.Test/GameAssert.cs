﻿using EighteenSeventeen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EighteenSeventeen.Test
{
    public static class GameAssert
    {
        public static void CurrentRoundIs(GameState gameState, string roundDescription)
        {
            Assert.Equal(roundDescription, gameState.Round.Description);
        }

        public static void PlayerHasPriority(GameState gameState, Player player)
        {
            Assert.Equal(player, gameState.PlayerWithPriority);
        }
    }
}
