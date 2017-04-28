using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RPGUI : MonoBehaviour {

	[SerializeField]
	Text Expa,
	Fuel,
	Strength,
	Exploration,
	Diplomacy,
	Management;

	[SerializeField]
	Button 
	FuelButton,
	StrengthButton,
	ExplorationButton,
	DiplomacyButton,
	ManagementButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		UpdateView ();
	}

	void UpdateView()
	{
		var parameters = PlayerController.Instance.Parameters;
		Expa.text = parameters.ExpreiencePoints.ToString ();
		Fuel.text = parameters.FuelMax.ToString ();
		Strength.text = parameters.StrengthCurrent.ToString ();
		Exploration.text = parameters.ExplorationCurrent.ToString ();
		Diplomacy.text = parameters.DiplomacyCurrent.ToString ();
		Management.text = parameters.ManagementCurrent.ToString ();

		bool canImprove = parameters.ExpreiencePoints > 0;
		FuelButton.gameObject.SetActive (canImprove);
		StrengthButton.gameObject.SetActive (canImprove);
		ExplorationButton.gameObject.SetActive (canImprove);
		ManagementButton.gameObject.SetActive (canImprove);
		DiplomacyButton.gameObject.SetActive (canImprove);
	}

	public void IncreaseFuel()
	{
		if (PlayerController.Instance.Parameters.ExpreiencePoints <= 0)
			return;
		PlayerController.Instance.Parameters.ExpreiencePoints--;
		PlayerController.Instance.Parameters.FuelMax += 1;
		UpdateView ();
		UIController.UpdateShipInfo ();
	}

	public void IncreaseStrength()
	{
		if (PlayerController.Instance.Parameters.ExpreiencePoints <= 0)
			return;
		PlayerController.Instance.Parameters.ExpreiencePoints--;
		PlayerController.Instance.Parameters.StrengthCurrent += 1;
		UpdateView ();
	}

	public void IncreaseExploration()
	{
		if (PlayerController.Instance.Parameters.ExpreiencePoints <= 0)
			return;
		PlayerController.Instance.Parameters.ExpreiencePoints--;
		PlayerController.Instance.Parameters.ExplorationCurrent += 1;
		UpdateView ();
	}

	public void IncreaseDiplomacy()
	{
		if (PlayerController.Instance.Parameters.ExpreiencePoints <= 0)
			return;
		PlayerController.Instance.Parameters.ExpreiencePoints--;
		PlayerController.Instance.Parameters.DiplomacyCurrent += 1;
		UpdateView ();
	}

	public void IncreaseManagement()
	{
		if (PlayerController.Instance.Parameters.ExpreiencePoints <= 0)
			return;
		PlayerController.Instance.Parameters.ExpreiencePoints--;
		PlayerController.Instance.Parameters.ManagementCurrent += 1;
		UpdateView ();
	}
}
