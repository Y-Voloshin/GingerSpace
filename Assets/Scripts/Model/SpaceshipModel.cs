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
		Vector3 Position;
		/// <summary>
		/// The rotation of transfor,; using for save/load
		/// </summary>
		Quaternion Rotation;
	}
}