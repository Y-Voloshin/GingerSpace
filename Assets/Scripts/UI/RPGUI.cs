using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Catopus.UI
{
    public class RPGUI : MonoBehaviour
    {

        [SerializeField]
        LabelValueGroup Expa;
        [SerializeField]
        RPGLabelValueGroup Fuel,
        Strength,
        Exploration,
        Diplomacy,
        Management;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            UpdateView();
        }

        void UpdateView()
        {
            if (PlayerController.Instance == null)
            {
                return;
            }

            bool canImprove = PlayerController.Instance.ExperiencePoints > 0;

            /*
            Expa.SetValueSafe(PlayerController.Instance.ExperiencePoints);
            Fuel.SetValueSafe(PlayerController.Instance.FuelMax, canImprove);
            Strength.SetValueSafe(PlayerController.Instance.StrengthCurrent, canImprove);
            Exploration.SetValueSafe(PlayerController.Instance.ExplorationCurrent, canImprove);
            Diplomacy.SetValueSafe(PlayerController.Instance.DiplomacyCurrent, canImprove);
            Management.SetValueSafe(PlayerController.Instance.ManagementCurrent, canImprove);
            */
            Expa.SetValueSafe(PlayerController.Instance.ExperiencePoints);
            Fuel.SetValueSafe(PlayerController.Instance.FuelMax, canImprove);
            Strength.SetValueSafe(BalanceParameters.GetBalancedStrength(), canImprove);
            Exploration.SetValueSafe(BalanceParameters.GetBalancedExploration(), canImprove);
            Diplomacy.SetValueSafe(BalanceParameters.GetBalancedDiplomacy(), canImprove);
            Management.SetValueSafe(PlayerController.Instance.ManagementCurrent, canImprove);
        }



        void IncreaseParameter(PlayerParameter p, int parameterPointsIncrease = 1, int ExperiencePointsDecrease = 1)
        {
            if (!PlayerController.Instance.SpendExperiencePointsOnParameter(p, parameterPointsIncrease, ExperiencePointsDecrease))
                return;
            UpdateView();
            UIController.UpdateShipInfo();
        }

        public void IncreaseFuel()
        {
            IncreaseParameter(PlayerParameter.FuelMax);
        }

        public void IncreaseStrength()
        {
            IncreaseParameter(PlayerParameter.StrengthCurrent);
        }

        public void IncreaseExploration()
        {
            IncreaseParameter(PlayerParameter.ExplorationCurrent);
        }

        public void IncreaseDiplomacy()
        {
            IncreaseParameter(PlayerParameter.DiplomacyCurrent);
        }

        public void IncreaseManagement()
        {
            IncreaseParameter(PlayerParameter.ManagementCurrent);
        }
    }
}