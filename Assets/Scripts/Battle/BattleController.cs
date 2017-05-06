using System;
using System.Collections;
using System.Collections.Generic;

namespace Catopus.Battle
{
    /// <summary>
    /// Боевая система
    /// </summary>
    public class BattleController : IBattleController
    {
        public event Action<BattleResultModel> OnBattleFinished;

        public void StartBattle(PlayerParameters playerParameters, int planetLevel)
        {
            BattleResultModel result = new BattleResultModel();

        }
    }
}