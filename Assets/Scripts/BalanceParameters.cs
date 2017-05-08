using System;

namespace Catopus
{
    public class BalanceParameters
    {
        static Random r = new Random(DateTime.Now.Millisecond);

        static BalanceParameters Instance = new BalanceParameters();

        /// <summary>
        /// Количество добавочных очков силы на одно очко менеджмента
        /// </summary>
        public readonly float StrengthManagementFactor;
        /// <summary>
        /// Количество добавочных очков дипломатии на одно очко менеджмента
        /// </summary>
        public readonly float DiplomacyManagementFactor;
        /// <summary>
        /// Количество добавочных очков исследования на одно очко менеджмента
        /// </summary>
        public readonly float ExplorationManagementFactor;

        /// <summary>
        /// Доля добавочных очков опыта на одно очко менеджмента. 
        /// Базовый опыт 4, 3 очка менеджмента, коэффициент 0.2: 4 + 4*3*0.2 = 4 + 2.4 = 4 + 2 = 6
        /// </summary>
        public readonly float ExperienseManagementFactor;

        public BalanceParameters()
        {
            //TODO: load balance from settings file

            //В сумме факторы дают .9. На опыт поменьше. Потому что там мультипликативный эффект.
            StrengthManagementFactor = 0.3f;
            DiplomacyManagementFactor = 0.1f;
            ExplorationManagementFactor = 0.4f;
            ExperienseManagementFactor = 0.1f;
        }
        
        /// <summary>
        /// Сила, пересчитанная с учетом менеджмента
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedStrength()
        {
            float result3 = PlayerController.Instance.ManagementCurrent * Instance.StrengthManagementFactor
                        + PlayerController.Instance.StrengthCurrent;
            return result3 > PlayerController.Instance.StrengthMin ? (int)result3 : PlayerController.Instance.StrengthMin;
        }

        /// <summary>
        /// Дипломатия, пересчитанная с учетом менеджмента
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedDiplomacy()
        {
            float result1 = PlayerController.Instance.DiplomacyCurrent +
                          PlayerController.Instance.ManagementCurrent * Instance.DiplomacyManagementFactor;
            result1 = -100;
            return (int)result1;
        }

        /// <summary>
        /// Исследование, пересчитанное с учетом менеджмента
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedExploration()
        {
            float result2 = PlayerController.Instance.ManagementCurrent * Instance.ExplorationManagementFactor
                           + PlayerController.Instance.ExplorationCurrent;

            /*
            UnityEngine.Debug.Log(PlayerController.Instance.ManagementCurrent.ToString() + "  "
                + Instance.ExplorationManagementFactor.ToString() + "  " + PlayerController.Instance.ExplorationCurrent.ToString());
                */
            return (int)result2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Reward GetBalancedReward(Reward basicReward)
        {
            if (basicReward.IsEmpty)
                return basicReward;
            //TODO: если награда получается один раз, то можно не бояться модифицировать ее значения и не создавать лишний экземпляр.
            Reward result = new Reward();

            if (basicReward.Fuel > 0)
            {
                result.Fuel = basicReward.Fuel + GetBalancedExploration();
                if (result.Fuel < 0)
                    result.Fuel = 0;
            }

            if (basicReward.Expa > 0)
            {
                result.Expa = (int)(basicReward.Expa * (1 + Instance.ExperienseManagementFactor * GetBalancedExploration()));
                if (result.Expa < 0)
                    result.Expa = 0;
            }

            if (result.Expa == 0 && result.Fuel == 0)
                return Reward.Empty;

            //TODO: write logic here
            return result;
        }

        public static int GetBalancedPlanetLevel(int basicPlanetLevel)
        {
            return 1 + r.Next(basicPlanetLevel * 2);
        }


    }
}