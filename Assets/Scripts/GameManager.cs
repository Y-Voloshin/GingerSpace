using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {Space, Quest, ScriptScene, Pause, MainMenu, InGameMenu}
public static class GameManager {
	public static GameState State = GameState.Space;

	#region planet functions, maybe put them in planet


	#endregion

	public static void TryStartQuest()
	{
		if (State != GameState.Space)
			return;
		//if 

	}


}
