using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using VGF;
using VGF.UintyUI;

namespace Catopus.UI
{
    public class UIController : MonoBehaviour
    {
        static UIController Instance;
        [SerializeField]
        ShipInfoUI ShipInfo;

        [SerializeField]
        GameObject QuestPanel;

        [SerializeField]
        Button 
            //ExplorePlanetButton,
        //LeavePlanetButton,
        GoToNearestPlanetButton,
        SaveGameButton; //TODO: put it in planet parameters form

        [SerializeField]
        QuestPanel QuestForm;
        [SerializeField]
        PlanetExploreResult RewardForm;
        [SerializeField]
        RPGUI RPGPanel;
        [SerializeField]
        BattleResultForm BattleResultForm;
        [SerializeField]
        CurrentPlanetInfo CurrentPlanetInfoForm;

        #region events
        public static event Action OnBattleResultPanelClosed,
            OnTryLeavePlanet,
            OnTryGoToNearestPlanet,

            OnSaveButtonPressed,
            OnLoadButtonPressed,
            OnRestartButtonPressed;

        #endregion

        // Use this for initialization
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        //TODO: put it in input controller
        void Update()
        {
            
        }

        #region planet functions

        public static void OnSpaceshipOnOrbit()
        {
            //if (Planet.Current.Visited)
            //    return;
            //Instance.ExplorePlanetButton.SetActivity(true);
            //Instance.LeavePlanetButton.SetActivity(true);
            Instance.GoToNearestPlanetButton.SetActivity(false);
            Instance.SaveGameButton.SetActivity(true);

            if (Instance.CurrentPlanetInfoForm != null)
                Instance.CurrentPlanetInfoForm.Show();
        }

        public static void OnCurrentPlanetObserved()
        {

        }

        static void ObservePlanet(Planet planet)
        {
            if (planet.Observed)
                return;
            planet.ObserveNewPlanetFirstTime();
        }        

        public static void VisitPlanet(Planet planet)
        {
            /*
            if (Instance.ExplorePlanetButton != null)
                Instance.ExplorePlanetButton.gameObject.SetActive(true);
                */
        }

        public static void OnCurrentPlanetLeft()
        {
            if (Instance.CurrentPlanetInfoForm != null)
                Instance.CurrentPlanetInfoForm.Hide();
            Instance.GoToNearestPlanetButton.SetActivity(true);
            UpdateShipInfo();
        }

        public static void ShowReward(Reward reward)
        {
            if (Instance.RewardForm == null)
                return;
            if (Instance.QuestPanel != null)
                Instance.QuestPanel.SetActive(true);
            Instance.RewardForm.gameObject.SetActive(true);

            Instance.RewardForm.Show(reward);
            UpdateShipInfo();
        }

        public static void ShowQuestResult(Reward reward, string message)
        {

            if (Instance.RewardForm == null)
                return;
            if (Instance.QuestPanel != null)
                Instance.QuestPanel.SetActive(true);
            Instance.RewardForm.gameObject.SetActive(true);

            Instance.RewardForm.Show(reward, message);
            UpdateShipInfo();
        }


        public static void ShowQuest(int questId)
        {
            if (Instance.QuestForm == null)
                return;
            if (Instance.QuestPanel != null)
                Instance.QuestPanel.SetActive(true);
            Instance.QuestForm.gameObject.SetActive(true);
            Instance.QuestForm.ShowQuest(questId);
        }

        #endregion

        #region battle functions
        public static void ShowBattleResult()
        {
            if (Instance.BattleResultForm != null)
                Instance.BattleResultForm.gameObject.SetActive(true);
        }

        public void CloseBattleResultPanel()
        {
            if (OnBattleResultPanelClosed != null)
                OnBattleResultPanelClosed();
        }
        #endregion

        public void ShowRPGPanel()
        {
            if (RPGPanel == null)
                return;
            RPGPanel.gameObject.SetActive(true);
        }

        public static void UpdateShipInfo()
        {
            if (Instance.ShipInfo != null)
                Instance.ShipInfo.UpdateValues();
        }

        #region abilities buttons

        public void LeavePlanetButtonPressed()
        {
            OnTryLeavePlanet.CallEventIfNotNull();
        }

        public void TryGoToNearestPlanet()
        {
            OnTryGoToNearestPlanet.CallEventIfNotNull();
        }

        public void VisitCurrentPlanet()
        {
            if (Planet.Current == null)
            {
                Debug.LogError("Try to explore null planet");
                return;
            }

            Planet.Current.VisitPlanet();
        }

        public void SaveGame()
        {
            OnSaveButtonPressed.CallEventIfNotNull();
        }

        public void LoadGame()
        {
            OnLoadButtonPressed.CallEventIfNotNull();
        }

        public void RestartGame()
        {
            OnRestartButtonPressed.CallEventIfNotNull();
        }

        #endregion
    }
}