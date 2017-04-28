using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spaceship : MonoBehaviour {	
	public static Spaceship Instance;

	[SerializeField]
	float Speed = 2;
	Dictionary<string, Action<Collider>> TagActions = new Dictionary<string, Action<Collider>>();

	Transform myTransform;

	//rotation args
	float angleSpeed;
	Transform planetTransform;
	Planet planet;

	public SpaceShipState State;

	[SerializeField]
	float fallingAngle;
	[SerializeField]
	Vector3 fallingAngleV3;
	[SerializeField]
	float fav3Magnitude;
	[SerializeField]
	float fallingDist;

	// Use this for initialization
	void Start () {
		TagActions.Add ("PlanetOrbit", OnPlanetOrbitEnter);
		TagActions.Add ("Finish", OnFinishEnter);

		myTransform = transform;
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.State != GameState.Space)
			return;

		if (Input.GetKeyDown (KeyCode.Space))
			Accelerate ();

		if (Input.GetKeyDown (KeyCode.LeftControl))
			GoToNearestPlanet ();

		switch (State) {
		case SpaceShipState.Idle:
			myTransform.Translate (Vector3.right * Speed * Time.deltaTime);
			break;
		case SpaceShipState.SettingOnOrbit:
			myTransform.Translate (Vector3.right * Speed * Time.deltaTime);
			CheckSettingOnOrbit ();
			break;
		case SpaceShipState.OnOrbit:
			myTransform.RotateAround (planetTransform.position, Vector3.forward, angleSpeed * Time.deltaTime);
			break;
		case SpaceShipState.Falling:
			var moveStep = myTransform.right * Speed * Time.deltaTime;
			myTransform.Translate (moveStep);
			fallingAngleV3 = Quaternion.FromToRotation (myTransform.right, planetTransform.position - myTransform.position).eulerAngles;
			//fallingAngle = Vector3.Angle (myTransform.right, planet.position - myTransform.position);
			if (fallingAngleV3.z > 200)
				fallingAngleV3.z = fallingAngleV3.z - 360;
			if (fallingAngleV3.z > 1f && fallingAngleV3.magnitude < 359)
				//myTransform.Rotate (0, 0, fallingAngle * 0.3f * Time.deltaTime);
				myTransform.Rotate (fallingAngleV3 * 0.3f * Time.deltaTime);
			else
				myTransform.Rotate (myTransform.forward, Quaternion.FromToRotation (myTransform.right, planetTransform.position - myTransform.position).eulerAngles.z);
			fallingDist = Vector3.Distance (myTransform.position, planetTransform.position);
			if (fallingDist < moveStep.magnitude)
				State = SpaceShipState.Dead;
			break;
		default:
			break;
		}

	}

	#region Interaction with world objects
	void OnTriggerEnter(Collider other)
	{
		string tag = other.tag;
		if (TagActions.ContainsKey (tag))
			TagActions [tag] (other);

	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("collision");
		/*
		string tag = collision.transform.tag;
		if (TagActions.ContainsKey (tag))
			TagActions [tag] (collision.collider);
			*/
	}

	void OnPlanetOrbitEnter(Collider planetCollider)
	{
		float angle = Vector3.Angle (myTransform.right, myTransform.position - planetCollider.transform.position);
		//Debug.Log (angle);


		Vector3 a = myTransform.right;
		Vector3 b = myTransform.position - planetCollider.transform.position;

		//float c = Vector3. (a, b);

		//Debug.Log (c);

		///*
		planet = planetCollider.GetComponent <Planet> ();
		this.planetTransform = planetCollider.transform;
		//
		if (angle > 140)
			DragToPlanet (planetCollider.transform);
		else
			StartSetToOrbit (planetCollider.transform);
			//*/
	}

	void OnFinishEnter(Collider finish)
	{
		finish.GetComponent <Finish>().ExecuteFinish ();
		State = SpaceShipState.Dead;
	}

	void DragToPlanet(Transform planet)
	{
		Debug.Log ("drag to planet");
		State = SpaceShipState.Falling;

		this.planetTransform = planet;
	}

	void SetToOrbit(Transform planet)
	{
		Debug.Log ("set to orbit");
		State = SpaceShipState.OnOrbit;

		float r = (myTransform.position - planet.position).magnitude;
		float l = 2 * Mathf.PI * r;

		angleSpeed = Speed / l * Mathf.PI * 100;
		this.planetTransform = planet;
	}

	void StartSetToOrbit(Transform planet)
	{
		Debug.Log ("start sto");
		State = SpaceShipState.SettingOnOrbit;

		float r = (myTransform.position - planet.position).magnitude;
		float l = 2 * Mathf.PI * r;

		angleSpeed = Speed / l * Mathf.PI * 100;
		this.planetTransform = planet;

		if (Vector3.Angle (myTransform.up, planetTransform.position - myTransform.position) > 90)
			angleSpeed = -angleSpeed;
	}

	void CheckSettingOnOrbit()
	{
		float angle = Vector3.Angle (myTransform.right, myTransform.position - planetTransform.position);
		if (angle < 90.1f) {
			State = SpaceShipState.OnOrbit;
			if (planet != null)
				planet.OnPlanetOrbitListener ();
		}
	}

	#endregion

	public void Accelerate()
	{
		if (State == SpaceShipState.OnOrbit) {
			if (!TryTakeFuel (1))
				return;
			State = SpaceShipState.Idle;
			if (planet != null)
				planet.LeavePlanet ();
		}
	}

	public void GoToNearestPlanet()
	{
		if (State != SpaceShipState.Idle)
			return;
		
		var p = Planet.GetClosest ();
		if (p == null)
			return;

		if (!TryTakeFuel (3))
			return;
		p.DirectSpaceshipToSelf ();
	}

	bool TryTakeFuel(int amount)
	{
		if (PlayerController.Instance.Parameters.FuelCurrent < amount)
			return false;
		PlayerController.Instance.Parameters.FuelCurrent -= amount;
		UIController.UpdateShipInfo ();
		return true;
	}

}

public enum SpaceShipState {Idle, SettingOnOrbit, OnOrbit, Falling, Dead}