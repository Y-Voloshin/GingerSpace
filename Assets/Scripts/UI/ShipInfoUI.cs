using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Catopus;

public class ShipInfoUI : MonoBehaviour {

	[SerializeField]
	Text Fuel, Expa;

	// Use this for initialization
	void Start () {
		UpdateValues ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateValues()
	{
		Fuel.text = PlayerController.Instance.FuelCurrent.ToString ()
		+ " / " + PlayerController.Instance.FuelMax.ToString ();

		int xp = PlayerController.Instance.ExperiencePoints;
		Expa.text = xp > 0 ? xp.ToString () : string.Empty;
	}
}
