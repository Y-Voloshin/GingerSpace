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
            SubscribeOnUIHotkeyEvents();
            SubscribeOnSpaceshipEvents();
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

        void SubscribeOnSpaceshipEvents()
        {
            Spaceship.Instance.OnPlanetEnteredInvalidAngle += OnPlanetEnteredInvalidAngleHandler;
            Spaceship.Instance.OnPlanetEnteredValidAngle += OnPlanetEnteredValidAngleHandler;
        }

        void SubscribeOnUIHotkeyEvents()
        {
            ButtonInput.InputController.OnAccelerateButtonDown += OnTryLeavePlanetHandler;
            UIController.OnTryLeavePlanet += OnTryLeavePlanetHandler;

            ButtonInput.InputController.OnGoToNearestPlanetButtonDown += OnTryGoToNearestPlanetHandler;
            UIController.OnTryGoToNearestPlanet += OnTryGoToNearestPlanetHandler;
        }

        #endregion

        public void SetState(GameState state)
        {

        }        

        #region UI command functions dublicated by input controller (commands with hotkeys)

        void OnTryLeavePlanetHandler()
        {
            int fuel = BalanceParameters.GetFuelForAcceleration();
            if (PlayerController.Instance.NotEnoughFuel(fuel))
                return;
            if (Spaceship.Instance.TryAccelerate())
            {
                PlayerController.Instance.TryTakeFuel(fuel);
                UIController.UpdateShipInfo();
            }
        }

        void OnTryGoToNearestPlanetHandler()
        {
            int fuel = BalanceParameters.GetFuelForGoingToNearestPlanet();
            if (PlayerController.Instance.NotEnoughFuel(fuel))
                return;
            if (Spaceship.Instance.TryGoToNearestPlanet())
            {
                PlayerController.Instance.TryTakeFuel(fuel);
                UIController.UpdateShipInfo();
            }
        }

        #endregion


        #region player parameter functions

        void OnTakeFuelHandler()
        {

        }

        void OnOutOfFuelHandler()
        {

        }

        void OnParametersUpdateHandler()
        {

        }

        #endregion

        #region spaceship functions

        void OnPlanetEnteredValidAngleHandler(Planet p)
        {
            Debug.Log(p);
            if (p == null)
                return;

            p.OnSpaceshipOnOrbit();
            //Update UI only after observation
            UIController.OnSpaceshipOnOrbit();
        }

        void OnPlanetEnteredInvalidAngleHandler(Planet p)
        {

        }

        void OnSetOnOrbitHandler()
        {

        }

        void OnPlanetLeftHandler()
        {

        }

        void OnLostInSpaceHandler()
        {

        }

        void OnCrashednPlanetHandler()
        {

        }

        #endregion

        #region observe planet function

        void OnPlanetObserveStartedHandler()
        {

        }

        void OnPlanetObservedHandler()
        {

        }

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