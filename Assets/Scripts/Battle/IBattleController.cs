using System;
using System.Collections;
using System.Collections.Generic;


namespace Catopus.Battle
{
    /// <summary>
    /// Battle conditions and rewards can vary for different places
    /// </summary>
    public enum BattlefieldType { Planet, Spaceship}
    /// <summary>
    /// Battle system can be rewritten but should realize this interface
    /// Интерфейс для боевой механики. Контроллер боя должен быть унаследован от этого интерфейса
    /// </summary>
    public interface IBattleController
    {
        /// ---
        /// Battle can be not immediate. So we have start function and end event. And in between it's just a blackbox
        /// 
        /// <para> Бой не обязательно автоматический и мгновенный. Поэтому сначала мы запускаем бой, а потом слушаем событие окончания боя.
        /// А между ними работает боевая система - и остальная игра о ее работе ничего не знает.</para>
        /// ---
        event Action OnBattleFinished;

        /// <summary>
        /// Start battle
        /// </summary>
        /// <param name="enemyLevel">Strength of enemy. For planet it's equal to planet level</param>
        /// <param name="battlefieldType">On planet you get only expa, enemy spaceship also gives you some fuel</param>
        void StartBattle(int enemyLevel, BattlefieldType battlefieldType);
    }
}