using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catopus.UI;
using VGF;
using Catopus.Model;

namespace Catopus
{
    public class Planet : GenericModelBehaviour<PlanetModel>
    {
        public static Planet Current;
        static Planet[] All;

        public float Radius { get { return CurrentModel.Radius; } }
        #region common parameters
        public int Level { get { return CurrentModel.Level; } }
        /// <summary>
        /// The observed. Была ли планета найдена
        /// </summary>
        public bool Observed { get { return CurrentModel.Observed; } }
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

#region check point

#endregion


//TODO: refactor all planets list
protected override void Awake()
        {
            All = null;
            base.Awake();
        }

        // Use this for initialization
        protected override void Start()
        {
            //HasQuest = true;
            if (All == null)
                All = FindObjectsOfType<Planet>();
        }
        

        public void OnPlanetOrbitListener()
        {
            Current = this;
            ObservePlanet();
            if (Visited)
                return;
            UIController.OnPlanetOrbitListener(this);
        }

        void ObservePlanet()
        {
            if (Observed)
                return;
            CurrentModel.Observed = true;
        }

        public void VisitPlanet()
        {
            if (!PlayerController.Instance.TryTakeFuel(1))
                return;

            UIController.UpdateShipInfo();

            if (Visited)
            {
                Debug.LogError("Trying visit planet which is already visited");
                return;
            }
            CurrentModel.Visited = true;
            if (HasQuest)
            {
                UIController.ShowQuest(QuestId);

            }
            else
            {
                //Если есть люди - шанс начать войну
                //Иначе - просто добываем ресурсы
                if (HasPopulation)
                {
                }
                else
                {
                    CurrentModel.Reward = GenerateResources();
                    if (!CurrentModel.Reward.IsEmpty)
                        PlayerController.Instance.ApplyReward(CurrentModel.Reward);

                    UIController.ShowReward(CurrentModel.Reward);

                }
            }
        }

        public void LeavePlanet()
        {
            UIController.LeavePlanet(this);
            Current = null;
        }

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
            Debug.Log(" == " + angle.ToString());
            Vector3 shipToPlanetV3 = transform.position - ship.position;
            float upAngle = Vector3.Angle(shipToPlanetV3, ship.up);
            if (upAngle < 90 || upAngle > 270)
                angle = -angle;
            Vector3 rotV3 = Quaternion.FromToRotation(ship.right, shipToPlanetV3).eulerAngles;
            //angle = Vector3.Angle (shipToPlanetV3, ship.right);
            Debug.Log(angle);

            angle += rotV3.z;
            ship.Rotate(Vector3.forward, angle);
        }

        public void SetRadius(float r)
        {
            CurrentModel.Radius = r;
        }

        public void Observe()
        {
            CurrentModel.Observed = true;
        }

        #region resources generation



        Reward GenerateResources()
        {
            if (!HasResources)
                return Reward.Empty;

            Reward result = new Reward();
            //min fuel + planet level fuel + exploration fuel
            int fuel = 2 + Random.Range(0, Level + PlayerController.Instance.ExplorationCurrent + 1);

            result.IsEmpty = false;
            result.Fuel = fuel;
            return result;
        }

        #endregion
    }


    public struct PlanetResources
    {
        public int FuelAmount;
    }
}