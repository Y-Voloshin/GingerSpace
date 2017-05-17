using System;

namespace Catopus
{
    /// <summary>
    /// Container for all the balance in game.
    /// <para>How strong does management influence on diplomaty?</para>
    /// <para>How much fuel do you need to observe a planet?</para>
    /// <para>At what angle can u enter a planet orbit?</para>
    /// <para>etc</para>
    /// </summary>
    public class BalanceParameters
    {
        //TODO: balance listens to parameters change and updates balanced values instead of calculating em each time
        static Random r = new Random(DateTime.Now.Millisecond);

        static BalanceParameters Instance = new BalanceParameters();

        public readonly float OrbitEnterMaxValidAngle;

        /// <summary>
        /// Strength: Quantity of additional points for one management point
        /// <para>Количество добавочных очков силы на одно очко менеджмента</para>
        /// </summary>
        public readonly float StrengthManagementFactor;
        /// <summary>
        /// Diplomacy: Quantity of additional points for one management point
        /// <para>Количество добавочных очков дипломатии на одно очко менеджмента</para> 
        /// </summary>
        public readonly float DiplomacyManagementFactor;

        /// <summary>
        /// Exploration: Quantity of additional points for one management point
        /// <para>Количество добавочных очков исследования на одно очко менеджмента</para>
        /// </summary>
        public readonly float ExplorationManagementFactor;

        /// <summary>
        /// Exploration: Piece of additional points for one management point
        /// <para>Basic xp is 4, management is 3, multiplier 0.2. Result: 4 (1 + 3 * 0.2) = 4 * 1.6 = 6.4 = 6</para>
        /// <para>Доля добавочных очков опыта на одно очко менеджмента. 
        /// <para>Базовый опыт 4, 3 очка менеджмента, коэффициент 0.2: 4 + 4*3*0.2 = 4 + 2.4 = 4 + 2 = 6</para>
        /// </para>
        /// </summary>
        public readonly float ExperienseManagementFactor;

        /// <summary>
        /// Frequency of planets with no quest
        /// <para>Частота планет без квестов</para>
        /// </summary>
        readonly float NoQuestProbablilityWeight;

        //TODO: add military planets probability

        public readonly int FuelForAcceleration;
        public readonly int FuelForGoingToNearestPlanet; //maybe rename to SetCourse
        public readonly int FuelForExploringPlanet;

        public BalanceParameters()
        {
            //TODO: load balance from settings file

            //Sum is 0.9. Not 1. Because for experience the effect is multiplicative, not addictive.
            //So experience effect is 0.1 instead of 0.2
            //We still can think that sum is 1, so one management point is equal to other parameters' points
            //В сумме факторы дают .9. На опыт поменьше. Потому что там мультипликативный эффект.
            StrengthManagementFactor = 0.3f;
            DiplomacyManagementFactor = 0.1f;
            ExplorationManagementFactor = 0.4f;
            ExperienseManagementFactor = 0.1f;
            NoQuestProbablilityWeight = 0.5f;

            FuelForAcceleration = 1;
            FuelForGoingToNearestPlanet = 3;
            FuelForExploringPlanet = 1;

            OrbitEnterMaxValidAngle = 140;
        }

        /// <summary>
        /// Strength, recalculated with management impact
        /// <para>Сила, пересчитанная с учетом менеджмента</para>
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedStrength()
        {
            float result3 = PlayerController.Instance.ManagementCurrent * Instance.StrengthManagementFactor
                        + PlayerController.Instance.StrengthCurrent;
            return result3 > PlayerController.Instance.StrengthMin ? (int)result3 : PlayerController.Instance.StrengthMin;
        }

        /// <summary>
        /// Diplomacy, recalculated with management impact
        /// <para>Дипломатия, пересчитанная с учетом менеджмента</para>
        /// </summary>
        /// <returns></returns>
        public static int GetBalancedDiplomacy()
        {
            float result1 = PlayerController.Instance.DiplomacyCurrent +
                          PlayerController.Instance.ManagementCurrent * Instance.DiplomacyManagementFactor;
            return (int)result1;
        }

        /// <summary>
        /// Exploration, recalculated with management impact
        /// <para>Исследование, пересчитанное с учетом менеджмента</para>
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
        
        public static int GetFuelForAcceleration() { return Instance.FuelForAcceleration; }
        public static int GetFuelForGoingToNearestPlanet() { return Instance.FuelForGoingToNearestPlanet; }
        public static int GetFuelForExploringPlanet() { return Instance.FuelForExploringPlanet; }


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

        public static int GetBalancedProbabilityValueForAssigningQuest(int questAmount)
        {
            return (int)(questAmount * (1 + Instance.NoQuestProbablilityWeight));
        }

        public static void GetBalancedDialogCasesProbability (out int good, out int neutral, out int bad)
        {
            neutral = good = bad = Planet.Current.Level;

            int bd = GetBalancedDiplomacy();
            //Level is 10. For diplomacy = 3: good = 13, bad = 10. For -3: good = 10, bad = 13
            //TODO: check other balanced parameters and do the same wy if needed. If increase is <0 and param + increase < 0,
            // then not param = 0, but anti-param -= increase
            if (bd > 0)
            {
                good += bd;
            }
            else
            {
                bad -= bd;
            }
        }

        public static float GetOrbitEnterMaxValidAngle()
        {
            return Instance.OrbitEnterMaxValidAngle;                 
        }
    }
}