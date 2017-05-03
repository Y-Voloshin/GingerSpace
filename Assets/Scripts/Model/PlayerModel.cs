using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VGF;

namespace Catopus.Model
{
    [System.Serializable]
    public class PlayerModel : AbstractModel<PlayerModel>
    {
        public int FuelMax = 10,
                FuelCurrent = 10,

                //Хоть что-то игрок всегда может сделать, так что сила у него минимум 1
                StrengthMin = 1,
                StrengthCurrent,

                DiplomacyMax,
                DiplomacyCurrent,//Нет максимума и минимума

                //Ну и поиск как и сила не может быть нулевой
                ExplorationMin = 1,
                ExplorationCurrent = 10,

                ManagementMax,
                ManagementCurrent,

                ExpreiencePoints;

        public int EmptyFuelPoints { get { return FuelMax - FuelCurrent; } }

        public PlayerModel() { }

        public PlayerModel(PlayerModel model)
        {
            SetValues(model);
        }

        public override void SetValues(PlayerModel model)
        {
            if (model == null)
                return;

            FuelMax = model.FuelMax;
            FuelCurrent = model.FuelCurrent;

            //Хоть что-то игрок всегда может сделать, так что сила у него минимум 1
            StrengthMin = model.StrengthMin;
            StrengthCurrent = model.StrengthCurrent;

            DiplomacyMax = model.DiplomacyMax;
            DiplomacyCurrent = model.DiplomacyCurrent;

            //Ну и поиск как и сила не может быть нулевой
            ExplorationMin = model.ExplorationMin;
            ExplorationCurrent = model.ExplorationCurrent;

            ManagementMax = model.ManagementMax;
            ManagementCurrent = model.ManagementCurrent;

            ExpreiencePoints = model.ExpreiencePoints;
        }

        public void SetValues(int fuelMax = 10, int fuelCurrent = 10, int strengthMin = 1,
                int explorationMin = 1, int explorationCurrent = 10)
        {
            FuelMax = fuelMax;
            FuelCurrent = fuelCurrent;
            StrengthMin = strengthMin;
            
            ExplorationMin = explorationMin;
            ExplorationCurrent = explorationCurrent;
        }

    }
}