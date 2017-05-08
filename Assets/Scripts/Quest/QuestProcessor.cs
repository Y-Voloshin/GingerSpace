using Catopus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProcessor : MonoBehaviour {
	public static QuestProcessor Instance;

	List<QuestPage> Quests = new List<QuestPage>();
	List<Dictionary<int, QuestPage>> QuestCases = new List<Dictionary<int, QuestPage>>();

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		int i = 0;
		var txt = Resources.Load<TextAsset> ("Quest" + i.ToString ());
		while (txt != null) {
			LoadQuest (txt);
			i++;
			txt = Resources.Load<TextAsset> ("Quest" + i.ToString ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadQuest (TextAsset txt)
	{
		var strings =  txt.text.Split ('\n');

		int LastQuestId = QuestCases.Count;

		foreach (var str in strings) {
			if (string.IsNullOrEmpty (str) || str.Length < 4)
				continue;
			var qp = new QuestPage (str);
			if (qp.IsStartPage) {
				Quests.Add (qp);
				//Добавляем запись в коллекцию квестовых диалогов
				QuestCases.Add (new Dictionary<int, QuestPage> ());
			} else {
				//Если для нового квеста создана запись в коллекции диалогов
				if (QuestCases.Count > LastQuestId) {
					if (!QuestCases [QuestCases.Count - 1].ContainsKey (qp.Id) )
						QuestCases [QuestCases.Count - 1].Add (qp.Id, qp);
				}
			}
		}
	}

	public void FinishQuest()
	{
		GameManager.State = GameState.Space;
	}

	public QuestPage GetQuest(int id)
	{
		if (Quests == null || Quests.Count <= id)
			return null;
		return Quests [id];
	}

	public QuestPage GetQuestPage(int questId, int pageId)
	{
		if (QuestCases != null && QuestCases.Count > questId
		    && QuestCases [questId] != null && QuestCases [questId].ContainsKey (pageId))
			return QuestCases [questId] [pageId];
		return null;
	}
}
