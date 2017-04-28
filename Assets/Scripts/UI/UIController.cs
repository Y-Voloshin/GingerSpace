using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	static UIController Instance;
	[SerializeField]
	ShipInfoUI ShipInfo;

	[SerializeField]
	GameObject QuestPanel;

	[SerializeField]
	Button ExplorePlanetButton,
	LeavePlanetButton,
	GoToNearestPlanetButton,
	SaveGameButton;

	[SerializeField]
	QuestPanel QuestForm;
	[SerializeField]
	PlanetExploreResult RewardForm;
	[SerializeField]
	RPGUI RPGPanel;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region planet functions

	public static void OnPlanetOrbitListener(Planet planet)
	{
		if (planet.Visited)
			return;
		if (Instance.ExplorePlanetButton != null)
			Instance.ExplorePlanetButton.gameObject.SetActive (true);
		if (Instance.LeavePlanetButton != null)
			Instance.LeavePlanetButton.gameObject.SetActive (true);
		if (Instance.GoToNearestPlanetButton != null)
			Instance.GoToNearestPlanetButton.gameObject.SetActive (false);
		if (Instance.SaveGameButton != null)
			Instance.SaveGameButton.gameObject.SetActive (true);
	}

	static void ObservePlanet(Planet planet)
	{
		if (planet.Observed)
			return;
		planet.Observed = true;
	}

	public static void VisitPlanet(Planet planet)
	{
		if (Instance.ExplorePlanetButton != null)
			Instance.ExplorePlanetButton.gameObject.SetActive (true);
	}

	public static void LeavePlanet(Planet planet)
	{
		if (Instance.ExplorePlanetButton != null)
			Instance.ExplorePlanetButton.gameObject.SetActive (false);
		if (Instance.LeavePlanetButton != null)
			Instance.LeavePlanetButton.gameObject.SetActive (false);
		if (Instance.GoToNearestPlanetButton != null)
			Instance.GoToNearestPlanetButton.gameObject.SetActive (true);
		if (Instance.SaveGameButton != null)
			Instance.SaveGameButton.gameObject.SetActive (false);
	}

	public static void ShowReward(Reward reward)
	{
		if (Instance.RewardForm == null)
			return;
		if (Instance.QuestPanel != null)
			Instance.QuestPanel.SetActive (true);
		Instance.RewardForm.gameObject.SetActive (true);

		Instance.RewardForm.Show (reward);
		UpdateShipInfo ();
	}

	public static void ShowQuestResult(Reward reward, string message)
	{
		
		if (Instance.RewardForm == null)
			return;
		if (Instance.QuestPanel != null)
			Instance.QuestPanel.SetActive (true);
		Instance.RewardForm.gameObject.SetActive (true);

		Instance.RewardForm.Show (reward, message);
		UpdateShipInfo ();
	}


	public static void ShowQuest(int questId)
	{
		if (Instance.QuestForm == null)
			return;
		if (Instance.QuestPanel != null)
			Instance.QuestPanel.SetActive (true);
		Instance.QuestForm.gameObject.SetActive (true);
		Instance.QuestForm.ShowQuest (questId);
	}
	#endregion

	public void ShowRPGPanel()
	{
		if (RPGPanel == null)
			return;
		RPGPanel.gameObject.SetActive (true);
	}

	public static void UpdateShipInfo()
	{
		if (Instance.ShipInfo != null)
			Instance.ShipInfo.UpdateValues ();
	}

	#region abilities buttons

	public void VisitCurrentPlanet()
	{
		if (Planet.Current == null) {
			Debug.LogError ("Try to explore null planet");
			return;
		}

		Planet.Current.VisitPlanet ();
	}

	public void SaveGame()
	{

	}

	#endregion
}
