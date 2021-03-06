﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catopus.Model;
using VGF;

namespace Catopus
{
    public class Spaceship : GenericModelBehaviour<SpaceshipModel>
    {
        public static Spaceship Instance;
        #region events
        public event Action<Planet> OnPlanetEnteredValidAngle,
                                    OnPlanetEnteredInvalidAngle;
        public event Action OnCurrentPlanetLeft;
        #endregion



        Dictionary<string, Action<Collider>> TagActions = new Dictionary<string, Action<Collider>>();

        Transform planetTransform;
        //Planet planet;
        float SpeedMultiplier = 1;

        public SpaceShipState State { get { return CurrentModel.State; } }

        // Use this for initialization
        protected void Start()
        {
            TagActions.Add("PlanetOrbit", OnPlanetOrbitEnter);
            TagActions.Add("Finish", OnFinishEnter);
            
            Instance = this;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (GameManager.State != GameState.Space)
                return;            

            switch (CurrentModel.State)
            {
                case SpaceShipState.Idle:
                    myTransform.Translate(Vector3.right * CurrentModel.Speed * Time.deltaTime * SpeedMultiplier);
                    break;
                case SpaceShipState.SettingOnOrbit:
                    myTransform.Translate(Vector3.right * CurrentModel.Speed * Time.deltaTime * SpeedMultiplier);
                    CheckSettingOnOrbit();
                    break;
                case SpaceShipState.OnOrbit:
                    myTransform.RotateAround(planetTransform.position, Vector3.forward, CurrentModel.AngularSpeed * Time.deltaTime * SpeedMultiplier);
                    break;
                case SpaceShipState.Falling:
                    FallOnPlanet();
                    break;
                default:
                    break;
            }

        }

        #region Interaction with world objects
        void OnTriggerEnter(Collider other)
        {
            if (TagActions.ContainsKey(other.tag))
                TagActions[other.tag](other);

        }        

        void OnPlanetOrbitEnter(Collider planetCollider)
        {
            float angle = Vector3.Angle(myTransform.right, myTransform.position - planetCollider.transform.position);
            
            Vector3 a = myTransform.right;
            Vector3 b = myTransform.position - planetCollider.transform.position;

            var planet = planetCollider.GetComponent<Planet>();
            this.planetTransform = planetCollider.transform;

            /*
            if (angle > 140)
                DragToPlanet(planetCollider.transform);
            else
                StartSetToOrbit(planetCollider.transform);
                */

            if (angle > 140)
            {
                OnPlanetEnteredInvalidAngle.CallEventIfNotNull(planet);
                //TODO: make it as callback from GameManager
                DragToPlanet(planetCollider.transform);
            }
            else
            {
                OnPlanetEnteredValidAngle.CallEventIfNotNull(planet);
                //TODO: make it as callback from GameManager
                StartSetToOrbit(planet.myTransform);
            }
        }

        void OnFinishEnter(Collider finish)
        {
            finish.GetComponent<Finish>().ExecuteFinish();
            //CurrentModel.State = SpaceShipState.Dead;
        }

        void DragToPlanet(Transform planet)
        {
            Debug.Log("drag to planet");
            CurrentModel.State = SpaceShipState.Falling;

            this.planetTransform = planet;
        }

        void FallOnPlanet()
        {
            var moveStep = myTransform.right * CurrentModel.Speed * Time.deltaTime;
            myTransform.Translate(moveStep);
            var fallingAngleV3 = Quaternion.FromToRotation(myTransform.right, planetTransform.position - myTransform.position).eulerAngles;
            //fallingAngle = Vector3.Angle (myTransform.right, planet.position - myTransform.position);
            if (fallingAngleV3.z > 200)
                fallingAngleV3.z = fallingAngleV3.z - 360;
            if (fallingAngleV3.z > 1f && fallingAngleV3.magnitude < 359)
                //myTransform.Rotate (0, 0, fallingAngle * 0.3f * Time.deltaTime);
                myTransform.Rotate(fallingAngleV3 * 0.3f * Time.deltaTime);
            else
                myTransform.Rotate(myTransform.forward, Quaternion.FromToRotation(myTransform.right, planetTransform.position - myTransform.position).eulerAngles.z);
            var fallingDist = Vector3.Distance(myTransform.position, planetTransform.position);
            if (fallingDist < moveStep.magnitude)
                CurrentModel.State = SpaceShipState.Dead;
        }

        void SetToOrbit(Transform planet)
        {
            Debug.Log("set to orbit");
            CurrentModel.State = SpaceShipState.OnOrbit;

            float r = (myTransform.position - planet.position).magnitude;
            float l = 2 * Mathf.PI * r;

            CurrentModel.AngularSpeed = CurrentModel.Speed / l * Mathf.PI * 100;
            this.planetTransform = planet;
        }

        void StartSetToOrbit(Transform planet)
        {
            //Debug.Log("start sto");
            CurrentModel.State = SpaceShipState.SettingOnOrbit;

            float r = (myTransform.position - planet.position).magnitude;
            float l = 2 * Mathf.PI * r;

            CurrentModel.AngularSpeed = CurrentModel.Speed / l * Mathf.PI * 100;
            this.planetTransform = planet;

            if (Vector3.Angle(myTransform.up, planetTransform.position - myTransform.position) > 90)
                CurrentModel.AngularSpeed = -CurrentModel.AngularSpeed;

            //OnPlanetEnteredValidAngle.CallEventIfNotNull(planet.GetComponent<Planet>());
        }

        void CheckSettingOnOrbit()
        {
            float angle = Vector3.Angle(myTransform.right, myTransform.position - planetTransform.position);
            if (angle < 90.1f)
            {
                CurrentModel.State = SpaceShipState.OnOrbit;
                //if (planet != null)
                //planet.OnPlanetOrbitListener();
                //OnPlanetEnteredValidAngle.CallEventIfNotNull();
            }
        }

        #endregion

        public void SetSpeedMultiplier(float m)
        {
            SpeedMultiplier = m;
        }

        public void Accelerate()
        {            
            /*
            if (Planet.Current != null)
                Planet.Current.LeavePlanet(); //TODO: put LeavePlanet logic in Spaceship
                */
            CurrentModel.State = SpaceShipState.Idle;
        }

        public bool TryGoToNearestPlanet()
        {
            if (State != SpaceShipState.Idle)
                return false;

            var p = Planet.GetClosest();
            if (p == null)
                return false;

            //TODO: direct spaceship to p from spaceship itself
            p.DirectSpaceshipToSelf();
            return true;
        }

        protected override void Init()
        {
            //print(CurrentModel);
            //print(myTransform);
            base.Init();
            //print(CurrentModel);
            //print(myTransform);
            CurrentModel.SetTransformParameters(myTransform);
        }

        protected override void Save()
        {
            CurrentModel.SetTransformParameters(myTransform);
            base.Save();
        }

        protected override void Load()
        {
            base.Load();
            ResetToCurrent();
        }

        protected override void LoadInit()
        {
            base.LoadInit();
            ResetToCurrent();
        }

        void ResetToCurrent()
        {
            myTransform.position = CurrentModel.Position;
            myTransform.rotation = CurrentModel.Rotation;            
        }

    }

    public enum SpaceShipState { Idle, SettingOnOrbit, OnOrbit, Falling, Dead }
}