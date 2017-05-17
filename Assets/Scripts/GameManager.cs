using UnityEngine;
using Catopus.Battle;
using Catopus.UI;
using Catopus.ButtonInput;
using VGF;

namespace Catopus
{
    public enum GameState { Space, Quest, ScriptScene, Pause, MainMenu, InGameMenu }
    /// <summary>
    /// "Bridge class" for event exchange among classes-systems that don't know about each other
    /// </summary>
    public class GameManager {
        public static GameState State = GameState.Space;

        public static IBattleController BattleController = new BattleController();
        
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
            SubscribeOnSaveLoadEvents();
        }
        #endregion

        #region subscribe on events
        void SubscribeOnSaveLoadEvents()
        {
            UIController.OnLoadButtonPressed += Load;
            UIController.OnRestartButtonPressed += LoadInit;
            UIController.OnSaveButtonPressed += Save;

            InputController.OnLoad += Load;
            InputController.OnLoadInit += LoadInit;
            InputController.OnSave += Save;
        }

        void SubscribeOnPlanetEvents()
        {
            Planet.OnCurrentPlanetConflictAppeared += OnCurrentPlanetConflictAppearedHandler;
            Planet.OnCurrentPlanetObserved += OnCurrentPlanetObservedHandler;
            Planet.OnCurrentPlanetQuestStarted += OnCurrentPlanetStartQuestHandler;
            Planet.OnPlanetResourcesExplored += OnPlanetResourcesExploredHandler;
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
            InputController.OnAccelerateButtonDown += OnTryLeavePlanetHandler;
            UIController.OnTryLeavePlanet += OnTryLeavePlanetHandler;

            InputController.OnGoToNearestPlanetButtonDown += OnTryGoToNearestPlanetHandler;
            UIController.OnTryGoToNearestPlanet += OnTryGoToNearestPlanetHandler;

            InputController.OnLMBDown += OnLMBDownHandler;
            InputController.OnLMBUp += OnLMBUpHandler;
        }

        #endregion

        public void SetState(GameState state)
        {

        }        

        #region UI command functions dublicated by input controller (commands with hotkeys)

        void OnTryLeavePlanetHandler()
        {
            if (!(Spaceship.Instance.State == SpaceShipState.OnOrbit
                || Spaceship.Instance.State == SpaceShipState.SettingOnOrbit))
                return;

            int fuel = BalanceParameters.GetFuelForAcceleration();
            if (PlayerController.Instance.NotEnoughFuel(fuel))
                return;

            Spaceship.Instance.Accelerate();
            //TODO: Create a function which calls GameOver if TryTakeFuel returns false
            //and use that function everywhere
            PlayerController.Instance.TryTakeFuel(fuel);
            UIController.OnCurrentPlanetLeft();
            //UIController.UpdateShipInfo();
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

        void OnLMBDownHandler()
        {
            if (Spaceship.Instance.State == SpaceShipState.OnOrbit
                || Spaceship.Instance.State == SpaceShipState.SettingOnOrbit)
            {
                ShipSlowDown();
            }
            //else
            //    OnTryGoToNearestPlanetHandler();
        }

        void OnLMBUpHandler()
        {
            if (Spaceship.Instance.State == SpaceShipState.OnOrbit
                || Spaceship.Instance.State == SpaceShipState.SettingOnOrbit)
            {
                ShipSpeedNormal();
                OnTryLeavePlanetHandler();
            }
            else
                OnTryGoToNearestPlanetHandler();
        }

        void ShipSlowDown()
        {
            Spaceship.Instance.SetSpeedMultiplier(0.3f);
        }

        void ShipSpeedNormal()
        {
            Spaceship.Instance.SetSpeedMultiplier(1);
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
            //Debug.Log(p);
            if (p == null)
                return;

            p.OnSpaceshipOnOrbit();
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

        void OnTryObservePlanetHandler()
        {
            if (Planet.Current.Visited)
            {
                Debug.LogError("WTF trying to visit already visited planet");
                return;
            }

            int fuel = BalanceParameters.GetFuelForGoingToNearestPlanet();
            if (PlayerController.Instance.NotEnoughFuel(fuel))
                return;
            //TODO: if not enough fuel that gameover
            PlayerController.Instance.TryTakeFuel(fuel);
            UIController.UpdateShipInfo();
            Planet.Current.VisitPlanet();
        }

        void OnPlanetObserveStartedHandler()
        {

        }

        void OnCurrentPlanetObservedHandler()
        {
            //Update UI only after observation
            UIController.OnSpaceshipOnOrbit();
        }

        void OnPlanetResourcesExploredHandler(Reward reward)
        {
            PlayerController.Instance.ApplyReward(reward);
            UIController.ShowReward(reward);
        }

        void OnCurrentPlanetConflictAppearedHandler()
        {
            //Debug.Log("Start battle: planet level is " + p.Level.ToString());
            BattleController.StartBattle(Planet.Current.Level, BattlefieldType.Planet);
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

        void OnExplorationResultFormClosed()
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

        #region quest functions

        void OnCurrentPlanetStartQuestHandler()
        {
            UIController.ShowQuest(Planet.Current.QuestId);
        }

        #endregion

        #region save load functions

        void Load()
        {
            SaveLoadBehaviour.LoadAll();
        }

        void LoadInit()
        {
            SaveLoadBehaviour.LoadInitAll();
        }

        public virtual void Save()
        {
            SaveLoadBehaviour.SaveAll();
        }

        #endregion


    }
}