﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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
        Button ExplorePlanetButton,
        LeavePlanetButton,
        GoToNearestPlanetButton,
        SaveGameButton;

        [SerializeField]
        QuestPanel QuestForm;
        [SerializeField]
        PlanetExploreResult RewardForm;
        [SerializeField]
        RPGUI RPGPanel;
        [SerializeField]
        BattleResultForm BattleResultForm;

        #region events
        public static event Action OnBattleResultPanelClosed;

        #endregion

        // Use this for initialization
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Spaceship.Instance.Save();
            if (Input.GetKeyDown(KeyCode.L))
                Spaceship.Instance.Load();
            if (Input.GetKeyDown(KeyCode.I))
                Spaceship.Instance.LoadInit();
        }

        #region planet functions

        public static void OnPlanetOrbitListener(Planet planet)
        {
            if (planet.Visited)
                return;
            Instance.ExplorePlanetButton.SetActivity(true);
            Instance.LeavePlanetButton.SetActivity(true);
            Instance.GoToNearestPlanetButton.SetActivity(false);
            Instance.SaveGameButton.SetActivity(true);
        }

        static void ObservePlanet(Planet planet)
        {
            if (planet.Observed)
                return;
            planet.Observe();
        }

        public static void VisitPlanet(Planet planet)
        {
            if (Instance.ExplorePlanetButton != null)
                Instance.ExplorePlanetButton.gameObject.SetActive(true);
        }

        public static void LeavePlanet(Planet planet)
        {
            Instance.ExplorePlanetButton.SetActivity(false);
            Instance.LeavePlanetButton.SetActivity(false);
            Instance.GoToNearestPlanetButton.SetActivity(true);
            Instance.SaveGameButton.SetActivity(false);
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
            Spaceship.Instance.Save();
        }

        public void LoadGame()
        {
            Spaceship.Instance.Load();
        }

        public void RestartGame()
        {
            Spaceship.Instance.LoadInit();
        }

        #endregion
    }
}