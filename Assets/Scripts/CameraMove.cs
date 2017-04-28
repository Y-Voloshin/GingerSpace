using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	Transform myTransform,
			ship;
	// Use this for initialization
	void Start () {
		ship = GameObject.FindObjectOfType<Spaceship> ().transform;
		if (ship == null)
			enabled = false;

		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (ship.position.x, ship.position.y, transform.position.z);
	}
}
