using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VGF;
using Catopus.Model;

namespace Catopus
{
    public enum PlayerParameter { FuelMax, FuelCurrent, StrengthMin, StrengthCurrent, DiplomacyMax,
        DiplomacyCurrent, ExplorationMin, ExplorationCurrent, ManagementMax, ManagementCurrent, ExpreiencePoints}

    public class PlayerController : GenericModelBehaviour<PlayerModel>
    {
        public static PlayerController Instance;

        public int FuelMax { get { return CurrentModel.FuelMax; } }
        public int FuelCurrent { get { return CurrentModel.FuelCurrent; } }
        public int StrengthMin { get { return CurrentModel.StrengthMin; } }
        public int StrengthCurrent { get { return CurrentModel.StrengthCurrent; } }
        public int DiplomacyMax { get { return CurrentModel.DiplomacyMax; } }
        public int DiplomacyCurrent { get { return CurrentModel.DiplomacyCurrent; } }
        public int ExplorationMin { get { return CurrentModel.ExplorationMin; } }
        public int ExplorationCurrent { get { return CurrentModel.ExplorationCurrent; } }
        public int ManagementMax { get { return CurrentModel.ManagementMax; } }
        public int ManagementCurrent { get { return CurrentModel.ManagementCurrent; } }
        public int ExperiencePoints { get { return CurrentModel.ExpreiencePoints; } }

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }

        public bool NotEnoughFuel(int fuelAmount)
        {
            return fuelAmount > FuelCurrent;
        }

        public bool TryTakeFuel(int fuelAmount)
        {
            if (fuelAmount > FuelCurrent)
                return false;
            CurrentModel.FuelCurrent -= fuelAmount;
            return true;
        }

        public void ApplyReward(Reward reward)
        {
            if (reward.IsEmpty)
                return;

            CurrentModel.FuelCurrent += reward.Fuel;
            if (CurrentModel.FuelCurrent > CurrentModel.FuelMax)
                CurrentModel.FuelCurrent = CurrentModel.FuelMax;
            else if (CurrentModel.FuelCurrent < 0)
                CurrentModel.FuelCurrent = 0;

            CurrentModel.StrengthCurrent += reward.Strength;
            if (CurrentModel.StrengthCurrent < CurrentModel.StrengthMin)
                CurrentModel.StrengthCurrent = CurrentModel.StrengthMin;

            CurrentModel.DiplomacyCurrent += reward.Diplomacy;

            CurrentModel.ExplorationCurrent += reward.Exploration;
            if (CurrentModel.ExplorationCurrent < CurrentModel.ExplorationMin)
                CurrentModel.ExplorationCurrent = CurrentModel.ExplorationMin;

            CurrentModel.ManagementCurrent += reward.Management;

            CurrentModel.ExpreiencePoints += reward.Expa;
        }

        public bool SpendExperiencePointsOnParameter(PlayerParameter p, int parameterPointsIncrease, int ExperiencePointsDecrease)
        {
            if (ExperiencePointsDecrease > ExperiencePoints)
                return false;
            CurrentModel.ExpreiencePoints -= ExperiencePointsDecrease;

            switch (p)
            {
                case PlayerParameter.DiplomacyCurrent:
                    CurrentModel.DiplomacyCurrent += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.DiplomacyMax:
                    CurrentModel.DiplomacyMax += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.ExplorationCurrent:
                    CurrentModel.ExplorationCurrent += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.ExplorationMin:
                    CurrentModel.ExplorationMin += ExperiencePointsDecrease;
                    break;
                //case PlayerParameter.ExpreiencePoints:
                //    break;
                case PlayerParameter.FuelCurrent:
                    CurrentModel.FuelCurrent += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.FuelMax:
                    CurrentModel.FuelMax += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.ManagementCurrent:
                    CurrentModel.ManagementCurrent += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.ManagementMax:
                    CurrentModel.ManagementMax += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.StrengthCurrent:
                    CurrentModel.StrengthCurrent += ExperiencePointsDecrease;
                    break;
                case PlayerParameter.StrengthMin:
                    CurrentModel.StrengthMin += ExperiencePointsDecrease;
                    break;

            }
            return true;
        }

    }
}
