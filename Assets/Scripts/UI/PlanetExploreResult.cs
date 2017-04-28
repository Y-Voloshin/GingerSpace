using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetExploreResult : MonoBehaviour {

	[SerializeField]
	Text Fuel,

	Strength,
	Diplomacy,
	Exploration,
	Management,
	ExpreiencePoints,

	Description;

	static string GotResources = "Вы нашли полезные ресурсы.",
				NoResources = "Вы не нашли ничего ценного.";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show (Reward reward, string description)
	{
		Description.text = description;
		if (reward.IsEmpty) {
			ShowNoReward ();
		}
		else {
			ShowGotResources (reward);
		}
	}

	public void Show(Reward reward)
	{
		if (reward.IsEmpty) {
			ShowNoReward ();
			Description.text = NoResources;
		}
		else {
			ShowGotResources (reward);
			Description.text = GotResources;
		}
		
	}

	void ShowGotResources(Reward reward)
	{
		ShowResource (Fuel, reward.Fuel);

		ShowResource (ExpreiencePoints, reward.Expa);
		ShowResource (Strength, reward.Strength);
		ShowResource (Diplomacy, reward.Diplomacy);
		ShowResource (Exploration, reward.Exploration);
		ShowResource (Management, reward.Management);
	}

	void ShowResource(Text textField, int value)
	{
		if (value == 0)
			textField.gameObject.SetActive (false);
		else {
			textField.gameObject.SetActive (true);
			textField.text = value.ToString ();
		}
	}

	void ShowNoReward()
	{
		Fuel.gameObject.SetActive (false);
		Strength.gameObject.SetActive (false);
		Diplomacy.gameObject.SetActive (false);
		Exploration.gameObject.SetActive (false);
		Management.gameObject.SetActive (false);
		ExpreiencePoints.gameObject.SetActive (false);
	}
}
