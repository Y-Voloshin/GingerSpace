using System;

namespace Catopus
{
    public class BalanceParameters
    {
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
        /// Only for inner player parameters: Diplomacy, Exploration and Strength. Not for expa or fuel from quests
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedPlayerParameter(PlayerParameter parameter)
        {
            if (PlayerController.Instance == null)
                return 0;

            switch (parameter)
            {
                case PlayerParameter.DiplomacyCurrent:
                    float result1 = PlayerController.Instance.DiplomacyCurrent +
                        PlayerController.Instance.ManagementCurrent * Instance.DiplomacyManagementFactor;
                    return (int)result1;
                case PlayerParameter.ExplorationCurrent:
                    float result2 = PlayerController.Instance.ManagementCurrent * Instance.ExplorationManagementFactor
                        + PlayerController.Instance.ExplorationCurrent;
                    return (int)result2;
                case PlayerParameter.StrengthCurrent:
                    float result3 = PlayerController.Instance.ManagementCurrent * Instance.StrengthManagementFactor
                        + PlayerController.Instance.StrengthCurrent;
                    return result3 > PlayerController.Instance.StrengthMin? (int)result3 : PlayerController.Instance.StrengthMin;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Reward GetBalancedReward(Reward basicReward)
        {
            if (basicReward.IsEmpty)
                return basicReward;
            Reward result = new Reward();

            //TODO: write logic here


            return result;
        }

    }
}