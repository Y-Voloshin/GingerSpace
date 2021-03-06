﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Catopus.Battle
{
    /// <summary>
    /// Боевая система
    /// </summary>
    public class BattleController : IBattleController
    {
        
        Random r = new Random(DateTime.Now.Millisecond);

        public event Action OnBattleFinished;
        string PlanetWin = "Вы успешно истребили коренное население планеты. Как в старые добрые времена!",
               PlanetLose = "Местные обитатели испортили ваше вторжение. Возможно, они просто русские.",
               SpaceshipWin = "Космические пираты - это не только ценный опыт, но и три-четыре забавные шкурки для таксидермиста.",
               SpaceshipLose = "Оказывается, из котосьминогов получаются отличные абажюры. Или эполеты. Или серьги - зависит от размеров победителя.";


        public void StartBattle(int enemyLevel, BattlefieldType battlefieldType = BattlefieldType.Planet)
        {
            int sumProbability = enemyLevel + BalanceParameters.GetBalancedStrength();
            var Victory = r.Next(sumProbability) >= enemyLevel;
            var Message = GetMessage(Victory, battlefieldType);
            var rew = Victory ? GenerateReward(enemyLevel, battlefieldType) : Reward.Empty;

            BattleResultModel.UpdateLastResult(battlefieldType, Victory, rew, Message);

            if (OnBattleFinished != null) OnBattleFinished();
        }

        Reward GenerateReward(int enemyLevel, BattlefieldType battlefieldType)
        {
            Reward result = new Reward();
            result.Expa = enemyLevel;
            if (battlefieldType == BattlefieldType.Spaceship)
                result.Fuel = r.Next(enemyLevel);
            return BalanceParameters.GetBalancedReward(result);
        }
        
        string GetMessage(bool victory, BattlefieldType battlefieldType)
        {
            if (victory)
                return (battlefieldType == BattlefieldType.Planet)? PlanetWin : SpaceshipWin;
            else
                return (battlefieldType == BattlefieldType.Planet) ? PlanetLose : SpaceshipLose;
        }
    }
}