//using System.Collections;
using System;
using System.Collections.Generic;
//using UnityEngine;

public enum DiplomacyResult {Neutral, Good, Bad}

public class QuestPage {
	static Random rand = new Random ();

	public readonly int Id;
	public readonly string Text;
	public readonly List<int> Cases = new List<int>();
	public readonly bool IsStartPage, IsEndPage;
	public DiplomacyResult DiplomacyType;

	public Reward Reward;

	public QuestPage(string str)
	{
		var words = str.Split ('\t');
		if (words == null || words.Length == 0)
			return;

		if (words [0] == "start") {
			IsStartPage = true;
		} else {
			var headWords = words [0].Split (' ');

			Id = Convert.ToInt32 (headWords [0]);
			if (Id == 0)
				return;

			for (int i = 1; i < headWords.Length; i++) {
				switch (headWords[i])
				{
					case "end":
						IsEndPage = true;
                        //UnityEngine.Debug.Log("is end page");
						break;
				case "good":
					DiplomacyType = DiplomacyResult.Good;
					break;
				case "bad":
					DiplomacyType = DiplomacyResult.Bad;
					break;
					default:
						break;
				}
			}
		}

		if (words.Length == 1)
			return;
		Text = words [1];

		if (words.Length == 2) {
			if (IsEndPage)
				Reward = Reward.Empty;
			return;
		}
		
		var casesWords = words [2].Split (' ');

		if (IsEndPage) {
			if (casesWords.Length < 1)
				return;
			
			//fuel expa strength explore manag diplom
			Reward = new Reward ();

			Reward.Fuel = Convert.ToInt32 (casesWords [0]);
			Reward.Expa = Convert.ToInt32 (casesWords [1]);
			Reward.Strength = Convert.ToInt32 (casesWords [2]);
			Reward.Exploration = Convert.ToInt32 (casesWords [3]);
			Reward.Management = Convert.ToInt32 (casesWords [4]);
			Reward.Diplomacy = Convert.ToInt32 (casesWords [5]);
		} else {
            foreach (var c in casesWords)
                try
                {
                    Cases.Add(Convert.ToInt32(c));
                }
                catch
                {
                    UnityEngine.Debug.Log(IsEndPage.ToString() + "   " + str);
                }
		}
	}

	public int GetRandomCaseId()
	{
		if (Cases == null || Cases.Count == 0)
			return -1;
		return Cases[rand.Next(Cases.Count )];
	}
}

