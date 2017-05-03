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
        public float Speed, AngularSpeed;

        public SpaceshipModel() { }


        //NOTE: remove later if unnecessary
        /* current commit is 4
        public SpaceshipModel(Vector3 pos, Quaternion rot, float speed, SpaceShipState s = SpaceShipState.Idle, float aSpeed = 0, int pId = -1)
        {
            SetValues(pos, rot, speed, s, aSpeed, pId);
        }
        */

        public SpaceshipModel(SpaceshipModel model)
        {
            SetValues(model);
        }

        public override void SetValues(SpaceshipModel model)
        {
            if (model == null)
                return;
            
            Position = model.Position;
            Rotation = model.Rotation;

            Speed = model.Speed;
            State = model.State;
            AngularSpeed = model.AngularSpeed;
            PlanetId = model.PlanetId;
        }

        public void SetTransformParameters(Transform tr)
        {
            Position = tr.position;
            Rotation = tr.rotation;
        }

    }
}