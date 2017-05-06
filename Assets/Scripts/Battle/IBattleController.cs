using System;
using System.Collections;
using System.Collections.Generic;


namespace Catopus.Battle
{
    /// <summary>
    /// Интерфейс для боевой механики. Контроллер боя должен быть унаследован от этого интерфейса
    /// 
    /// </summary>
    public interface IBattleController
    {
        /// ---
        /// Бой не обязательно автоматический и мгновенный. Поэтому сначала мы запускаем бой, а потом слушаем событие окончания боя.
        /// А между ними работает боевая система - и остальная игра о ее работе ничего не знает.
        /// ---
        event Action<BattleResultModel> OnBattleFinished;

        /// <summary>
        /// запуск боя
        /// </summary>
        void StartBattle(PlayerParameters playerParameters, int planetLevel);
    }
}