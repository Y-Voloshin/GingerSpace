using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController Instance;
	public PlayerParameters Parameters = new PlayerParameters();

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ApplyReward(Reward reward)
	{
		if (reward.IsEmpty)
			return;

		Parameters.FuelCurrent += reward.Fuel;
		if (Parameters.FuelCurrent > Parameters.FuelMax)
			Parameters.FuelCurrent = Parameters.FuelMax;
		else if (Parameters.FuelCurrent < 0)
			Parameters.FuelCurrent = 0;

		Parameters.StrengthCurrent += reward.Strength;
		if (Parameters.StrengthCurrent < Parameters.StrengthMin)
			Parameters.StrengthCurrent = Parameters.StrengthMin;

		Parameters.DiplomacyCurrent += reward.Diplomacy;

		Parameters.ExplorationCurrent += reward.Exploration;
		if (Parameters.ExplorationCurrent < Parameters.ExplorationMin)
			Parameters.ExplorationCurrent = Parameters.ExplorationMin;

		Parameters.ManagementCurrent += reward.Management;

		Parameters.ExpreiencePoints += reward.Expa;
	}

	//public void 
}
