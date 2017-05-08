using UnityEngine;
using Catopus.Battle;
using Catopus.UI;

namespace Catopus
{
    public enum GameState { Space, Quest, ScriptScene, Pause, MainMenu, InGameMenu }
    /// <summary>
    /// "Bridge class" for event exchange among classes-systems that don't know about each other
    /// </summary>
    public class GameManager {
        public static GameState State = GameState.Space;

        public static IBattleController BattleController = new BattleController();


        #region planet functions, maybe put them in planet


        #endregion

        public static void TryStartQuest()
        {
            if (State != GameState.Space)
                return;
            //if 

        }

        #region instance used for initialization
        private static GameManager Instance = new GameManager();
        GameManager()
        {
            SubscribeOnPlanetEvents();
            SubscribeOnBattleEvents();
            SubscribeOnUIEvents();
        }
        #endregion

        #region subscribe on events
        void SubscribeOnPlanetEvents()
        {
            var planets = GameObject.FindObjectsOfType<Planet>();
            foreach (var p in planets)
            {
                //Debug.Log("subscribe on planet event");
                p.OnConflictAppeared += OnPlanetConflictAppearedHandler;
            }
        }

        void SubscribeOnBattleEvents()
        {
            BattleController.OnBattleFinished += OnBattleFinishedHandler;
        }

        void SubscribeOnUIEvents()
        {
            UIController.OnBattleResultPanelClosed += OnBattleResultPanelClosedHandler;
        }

        #endregion


        #region observe planet function
        void OnPlanetConflictAppearedHandler(Planet p)
        {
            Debug.Log("Start battle: planet level is " + p.Level.ToString());
            BattleController.StartBattle(p.Level, BattlefieldType.Planet);
            //Debug.Log("sbhe-----");
        }
        #endregion


        #region battle functions
        void OnBattleFinishedHandler()
        {
            PlayerController.Instance.ApplyReward(BattleResultModel.LastBattleResult.Reward);
            UIController.ShowBattleResult();
        }

        #endregion

        #region unsorted UI command functions        

        void OnExplorePlanetForResourcesFinishedHandler()
        {

        }

        #endregion

        #region functions after battle

        void OnBattleResultPanelClosedHandler()
        {
            if (BattleResultModel.LastBattleResult.Victory)
            {
                if (BattleResultModel.LastBattleResult.BattlefieldType == BattlefieldType.Planet)
                    OnWinPlanetBattleHandler();
                else
                    OnWinSpaceshipBattleHandler();
            }
            else
            {
                if (BattleResultModel.LastBattleResult.BattlefieldType == BattlefieldType.Planet)
                    OnLosePlanetBattleHandler();
                else
                    OnLoseSpaceshipBattleHandler();
            }
        }

        void OnWinPlanetBattleHandler()
        {
            Planet.Current.ExploreForResoures();
        }

        void OnLosePlanetBattleHandler()
        {

        }

        void OnWinSpaceshipBattleHandler()
        {

        }

        void OnLoseSpaceshipBattleHandler()
        {

        }

        #endregion


    }
}