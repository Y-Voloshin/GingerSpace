using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverStupid : MonoBehaviour {
	[SerializeField]
	Text GameOverCase;
	[SerializeField]
	GameObject GameOverWindow;

	string caseLost = "Вы сбились с курса и затерялись в межзвездном пространстве.",
		caseBreakdown = "Вы не справились с управлением и врезались в планету",
		caseOutOfFuel = "В жизни поправимо всё. Всё корме пустого топливного бака в открытом космосе.";

	Planet[] planets;


	// Use this for initialization
	void Start () {
		planets = FindObjectsOfType <Planet> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate()
	{
		bool toofar = true;

		foreach (var p in planets) {
			float d = Vector3.Distance (Spaceship.Instance.transform.position, p.transform.position);
			if (d < 70) {
				toofar = false;
			}
		}

		if (toofar) {
			GameOverWindow.SetActive (true);
			GameOverCase.text = caseLost;
		} else if (Spaceship.Instance.State == SpaceShipState.Dead) {
			GameOverWindow.SetActive (true);
			GameOverCase.text = caseBreakdown;
		}
	}

	public void Restart()
	{
		//
		SceneManager.LoadScene (0);
	}

	public void Exit()
	{
		Application.Quit ();
	}

	public void CheckOutOfFuel()
	{
		if (PlayerController.Instance.Parameters.FuelCurrent == 0) {
			GameOverWindow.SetActive (true);
			GameOverCase.text = caseOutOfFuel;
		}
	}
}
