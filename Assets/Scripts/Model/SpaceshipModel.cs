using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catopus.Model
{
	[System.Serializable]
	public class SpaceshipModel {
		/// <summary>
		/// The position of transform; using for save/load
		/// </summary>
		public Vector3 Position { get; private set; }
		/// <summary>
		/// The rotation of transfor,; using for save/load
		/// </summary>
		public Quaternion Rotation { get; private set; }

        public SpaceShipState State;
        public int PlanetId;
        public float Speed, AngleSpeed;

        public SpaceshipModel(Vector3 pos, Quaternion rot, float speed, SpaceShipState s = SpaceShipState.Idle, float aSpeed = 0, int pId = -1)
        {
            SetValues(pos, rot, speed, s, aSpeed, pId);
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

    }
}