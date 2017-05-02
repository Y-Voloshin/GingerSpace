using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VGF;

namespace Catopus.Model
{
	[System.Serializable]
	public class SpaceshipModel : AbstractModel<SpaceshipModel> {

        ///*
        /// <summary>
        /// The position of transform; using for save/load
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// The rotation of transfor,; using for save/load
        /// </summary>
        public Quaternion Rotation;
        //*/
        /*
        /// <summary>
		/// The position of transform; using for save/load
		/// </summary>
		public Vector3 Position { get; private set; }
		/// <summary>
		/// The rotation of transfor,; using for save/load
		/// </summary>
		public Quaternion Rotation { get; private set; }
        */


        public SpaceShipState State;
        public int PlanetId;
        public float Speed, AngleSpeed;

        public SpaceshipModel() { }

        public SpaceshipModel(Vector3 pos, Quaternion rot, float speed, SpaceShipState s = SpaceShipState.Idle, float aSpeed = 0, int pId = -1)
        {
            SetValues(pos, rot, speed, s, aSpeed, pId);
        }

        public SpaceshipModel(SpaceshipModel model)
        {
            SetValues(model);
        }

        public override void SetValues(SpaceshipModel model)
        {
            Debug.Log(model);
            Debug.Log(this);
            if (model == null)
                return;
            SetValues(model.Position, model.Rotation, model.Speed, model.State, model.AngleSpeed, model.PlanetId);
        }

        public void SetValues(Vector3 pos, Quaternion rot, float speed, SpaceShipState s = SpaceShipState.Idle, float aSpeed = 0, int pId = -1)
        {
            Position = pos;
            Rotation = rot;

            Speed = speed;
            State = s;
            AngleSpeed = aSpeed;
            PlanetId = pId;
        }

        public void SetTransformParameters(Transform tr)
        {
            Position = tr.position;
            Rotation = tr.rotation;
        }

    }
}