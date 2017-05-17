using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	Transform myTransform,
			ship;
	// Use this for initialization
	void Start () {
		ship = GameObject.FindObjectOfType<Catopus.Spaceship> ().transform;
		if (ship == null)
			enabled = false;

		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 targetPos = Catopus.Spaceship.Instance.State == Catopus.SpaceShipState.OnOrbit ?
            Catopus.Planet.Current == null? ship.position :
            Catopus.Planet.Current.myTransform.position : ship.position;
        targetPos.z = myTransform.position.z;

        myTransform.position = Vector3.Lerp(myTransform.position, targetPos, Time.deltaTime);
	}
}
