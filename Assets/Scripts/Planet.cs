using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Catopus.UI;
using VGF;
using Catopus.Model;
//using Catopus.UI;

namespace Catopus
{
    public class Planet : GenericModelBehaviour<PlanetModel>
    {
        static System.Random r = new System.Random(System.DateTime.Now.Millisecond);

        public static Planet Current;
        static Planet[] All;

        

        public float Radius { get { return CurrentModel.Radius; } }
        #region common parameters
        public int Level { get { return CurrentModel.Level; } }
        /// <summary>
        /// The observed. Была ли планета найдена
        /// </summary>
        public bool Observed { get { return CurrentModel.Observed; } }
        public bool LevelObserved { get { return CurrentModel.LevelObserved; } }
        public bool PopulationObserved { get { return CurrentModel.PopulationObserved; } }
        public bool ResourcesObserved { get { return CurrentModel.ResourcesObserved; } }


        /// <summary>
        /// The visited. Была ли планета посещена? Планету можно посетить один раз.
        /// </summary>
        public bool Visited { get { return CurrentModel.Visited; } }
        public bool HasPopulation { get { return CurrentModel.HasPopulation; } }
        public bool HasResources { get { return CurrentModel.HasResources; } }
        #endregion

        #region quest parameters
        //згидшс Reward Reward { get { return CurrentModel.Reward; } }
        public bool HasQuest { get { return CurrentModel.HasQuest; } }
        public bool QuestCompleted { get { return CurrentModel.QuestCompleted; } }

        public int QuestId { get { return CurrentModel.QuestId; } }
        #endregion

        public bool RandomizeParameters = true;

        #region events
        public static event Action OnCurrentPlanetObserved,
            OnCurrentPlanetQuestStarted,
            OnCurrentPlanetConflictAppeared;
        public static event Action<Reward> OnPlanetResourcesExplored;

        #endregion        

        //TODO: refactor all planets list
        protected override void Awake()
        {
            All = null;
            base.Awake();
        }

        // Use this for initialization
        protected void Start()
        {
            //HasQuest = true;
            if (All == null)
                All = FindObjectsOfType<Planet>();
            if (RandomizeParameters)
            {
                Randomize();
                Init();
            }
        }
        
        void Randomize()
        {
            r = new System.Random(DateTime.Now.Millisecond);
            InitModel.HasResources = r.Next(2) > 0;
            InitModel.HasPopulation = r.Next(2) > 0;
            InitModel.HasQuest = Quest.QuestProcessor.GetRandomQuestId(ref InitModel.QuestId);
            if (InitModel.HasQuest)
                InitModel.HasPopulation = true;
        }

        public void OnSpaceshipOnOrbit()
        {
            Current = this;
            if (Observed)
                return;
            ObserveNewPlanetFirstTime();
        }
                
        /// <summary>
        /// Maybe we will add re-observe for fuel.
        /// For example: we visit planet second time after improving exploration.
        /// We can expect that we will re-obsere it.
        /// Or maybe we will re-observe planet just every visit.
        /// </summary>
        public void ObserveNewPlanetFirstTime()
        {
            CurrentModel.Observed = true;
            int exploration = BalanceParameters.GetBalancedExploration();
            if (exploration > 0)
            {
                int ObserveProbabilitySum = Level + exploration;
                CurrentModel.LevelObserved = r.Next(ObserveProbabilitySum) >= Level;
                CurrentModel.ResourcesObserved = r.Next(ObserveProbabilitySum) >= Level;
                CurrentModel.PopulationObserved = r.Next(ObserveProbabilitySum) >= Level;
            }
            OnCurrentPlanetObserved.CallEventIfNotNull();
        }

        public void VisitPlanet()
        {            
            if (Visited)
            {
                Debug.LogError("Trying visit planet which is already visited");
                return;
            }
            CurrentModel.Visited = true;
            ObserveEverything();

            if (HasQuest)
            {
                OnCurrentPlanetQuestStarted.CallEventIfNotNull();
            }
            else
            {
                //Если есть люди - шанс начать войну
                //Иначе - просто добываем ресурсы
                if (HasPopulation)
                {
                    //for (int i = 0; i < 50; i++)
                    InteractWithPopulation();
                }
                else
                {
                    ExploreForResoures();
                }
            }
        }

        void InteractWithPopulation()
        {
            //conflict chance is 50/50
            //And each diplomacy point increases pacific chance
            //So conflict chance is planetLevel and pacific chance is planetLevel  diplomacy
            int peaceProb = Level + BalanceParameters.GetBalancedDiplomacy();
            bool conflict = (peaceProb <= 0) || r.Next(Level + peaceProb) < Level;
            //conflict = true; // debug

            //TODO: not parameter, planet.Current instead
            if (conflict)
                OnCurrentPlanetConflictAppeared.CallEventIfNotNull();
        }

        public void ExploreForResoures()
        {
            //It can be called after battle by external class
            //TODO: But why do we put reward in current model?
            CurrentModel.Reward = GenerateResources();

            OnPlanetResourcesExplored.CallEventIfNotNull(CurrentModel.Reward);
        }

        void ObserveEverything()
        {
            CurrentModel.LevelObserved = true;
            CurrentModel.ResourcesObserved = true;
            CurrentModel.PopulationObserved = true;
            OnCurrentPlanetObserved.CallEventIfNotNull();
        }

        #region move to planet functions
        
        public static Planet GetClosest()
        {
            if (All == null || All.Length == 0)
                return null;
            Vector3 shipPos = Spaceship.Instance.transform.position;
            float dist = Vector3.Distance(All[0].transform.position, shipPos);
            Planet result = All[0];
            for (int i = 1; i < All.Length; i++)
            {
                float d = Vector3.Distance(All[i].transform.position, shipPos);
                if (d < dist)
                {
                    dist = d;
                    result = All[i];
                }
            }
            return result;
        }

        public void DirectSpaceshipToSelf()
        {
            var ship = Spaceship.Instance.transform;
            var v = ship.position;
            float dist = Vector3.Distance(transform.position, ship.position);
            float angle = Mathf.Asin(Radius / dist) * 180 / Mathf.PI;
            //Debug.Log(" == angle : " + angle.ToString() + "  " + Radius.ToString());
            Vector3 shipToPlanetV3 = transform.position - ship.position;
            float upAngle = Vector3.Angle(shipToPlanetV3, ship.up);
            if (upAngle < 90 || upAngle > 270)
                angle = -angle;
            Vector3 rotV3 = Quaternion.FromToRotation(ship.right, shipToPlanetV3).eulerAngles;
            //angle = Vector3.Angle (shipToPlanetV3, ship.right);
            //Debug.Log(angle);

            angle += rotV3.z;
            //Debug.Log(" == angle + rotV3 " + angle.ToString());
            ship.Rotate(Vector3.forward, angle);
        }

        #endregion

        public void SetRadius(float r)
        {
            InitModel.Radius = r;
        }

        

        #region resources generation



        Reward GenerateResources()
        {
            if (!HasResources)
                return Reward.Empty;

            Reward result = new Reward();
            //min fuel + planet level fuel + exploration fuel
            result.Fuel = 2 + r.Next(0, Level);// + PlayerController.Instance.ExplorationCurrent + 1);

            result = BalanceParameters.GetBalancedReward(result);
            return result;
        }

        #endregion
    }

    /*
    public struct PlanetResources
    {
        public int FuelAmount;
    }
    */
}