using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Catopus.UI
{
    public class RPGUI : MonoBehaviour
    {

        [SerializeField]
        Text Expa,
        Fuel,
        Strength,
        Exploration,
        Diplomacy,
        Management;

        [SerializeField]
        Button
        FuelButton,
        StrengthButton,
        ExplorationButton,
        DiplomacyButton,
        ManagementButton;

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
            Expa.text = PlayerController.Instance.ExperiencePoints.ToString();
            Fuel.text = PlayerController.Instance.FuelMax.ToString();
            Strength.text = PlayerController.Instance.StrengthCurrent.ToString();
            Exploration.text = PlayerController.Instance.ExplorationCurrent.ToString();
            Diplomacy.text = PlayerController.Instance.DiplomacyCurrent.ToString();
            Management.text = PlayerController.Instance.ManagementCurrent.ToString();

            bool canImprove = PlayerController.Instance.ExperiencePoints > 0;
            FuelButton.gameObject.SetActive(canImprove);
            StrengthButton.gameObject.SetActive(canImprove);
            ExplorationButton.gameObject.SetActive(canImprove);
            ManagementButton.gameObject.SetActive(canImprove);
            DiplomacyButton.gameObject.SetActive(canImprove);
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