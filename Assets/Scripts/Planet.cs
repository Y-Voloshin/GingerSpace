using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	public static Planet Current;
	static Planet[] All;
	public float Radius;
	#region common parameters
	public int Level = 1;
	/// <summary>
	/// The observed. Была ли планета найдена
	/// </summary>
	public bool Observed,
	/// <summary>
	/// The visited. Была ли планета посещена? Планету можно посетить один раз.
	/// </summary>
				Visited,
				HasPopulation,
				HasResources;
	#endregion

	#region quest parameters
	Reward Reward = new Reward();
	public bool HasQuest,
				QuestCompleeted;

	public int QuestId;
	#endregion

	#region check point

	#endregion

	void Awake()
	{
		All = null;
	}

	// Use this for initialization
	void Start () {
		//HasQuest = true;
		if (All == null)
			All = FindObjectsOfType <Planet> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPlanetOrbitListener()
	{
		Current = this;
		ObservePlanet ();
		if (Visited)
			return;
		UIController.OnPlanetOrbitListener (this);
	}

	void ObservePlanet()
	{
		if (Observed)
			return;
		Observed = true;
	}

	public void VisitPlanet()
	{
		if (PlayerController.Instance.Parameters.FuelCurrent <= 0)
			return;
		PlayerController.Instance.Parameters.FuelCurrent--;
		UIController.UpdateShipInfo ();

		if (Visited) {
			Debug.LogError ("Trying visit planet which is already visited");
			return;
		}
		Visited = true;
		if (HasQuest) {
			UIController.ShowQuest (QuestId);

		} else {
			//Если есть люди - шанс начать войну
			//Иначе - просто добываем ресурсы
			if (HasPopulation) {
			} else {
				Reward = GenerateResources ();
				if (!Reward.IsEmpty)
					PlayerController.Instance.ApplyReward (Reward);

				UIController.ShowReward (Reward);

			}
		}
	}

	public void LeavePlanet()
	{
		UIController.LeavePlanet (this);
		Current = null;
	}

	public static Planet GetClosest()
	{
		if (All == null || All.Length == null)
			return null;
		Vector3 shipPos = Spaceship.Instance.transform.position;
		float dist = Vector3.Distance (All [0].transform.position, shipPos);
		Planet result = All [0];
		for (int i = 1; i < All.Length; i++) {
			float d = Vector3.Distance (All [i].transform.position, shipPos);
			if (d < dist) {
				dist = d;
				result = All [i];
			}
		}
		return result;
	}

	public void DirectSpaceshipToSelf()
	{
		var ship = Spaceship.Instance.transform;
		var v = ship.position;
		float dist = Vector3.Distance (transform.position, ship.position);
		float angle = Mathf.Asin (Radius / dist) * 180 / Mathf.PI;
		Debug.Log (" == " + angle.ToString ());
		Vector3 shipToPlanetV3 = transform.position - ship.position;
		float upAngle = Vector3.Angle (shipToPlanetV3, ship.up);
		if ( upAngle < 90 || upAngle > 270)
			angle = -angle;
		Vector3 rotV3 = Quaternion.FromToRotation (ship.right, shipToPlanetV3).eulerAngles;
		//angle = Vector3.Angle (shipToPlanetV3, ship.right);
		Debug.Log (angle);

		angle += rotV3.z;
		ship.Rotate (Vector3.forward, angle);
	}

	#region resources generation



	Reward GenerateResources()
	{
		if (!HasResources)
			return Reward.Empty;

		Reward result = new Reward ();
		//min fuel + planet level fuel + exploration fuel
		int fuel = 2 + Random.Range (0, Level + PlayerController.Instance.Parameters.ExplorationCurrent + 1);
		fuel = Mathf.Min (fuel, PlayerController.Instance.Parameters.EmptyFuelPoints);

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