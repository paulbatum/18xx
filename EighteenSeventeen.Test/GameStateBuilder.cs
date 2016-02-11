﻿using EighteenSeventeen.Core;
using EighteenSeventeen.Core.GameSteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenSeventeen.Test
{
    public class GameStateBuilder
    {
        private Game Game { get; }

        public GameStateBuilder(Game game)
        {
            Game = game;
        }

        public void EachPlayerPasses()
        {
            foreach (var p in Game.Players)
                Game.Steps.Add(new PlayerPassStep(p));
        }

        public void PlayerBidsOnPrivate(string playerName, PrivateCompany privateCompany, int bid)
        {
            var step = new PlayerPrivateBidStep(Game.GetPlayer(playerName), privateCompany, bid);
            Game.Steps.Add(step);
        }
    }
}