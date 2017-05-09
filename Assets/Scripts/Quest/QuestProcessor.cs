﻿using Catopus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catopus.Quest
{
    public class QuestProcessor
    {
        static QuestProcessor Instance = new QuestProcessor();

        List<QuestPage> Quests = new List<QuestPage>();
        List<Dictionary<int, QuestPage>> QuestCases = new List<Dictionary<int, QuestPage>>();

        static System.Random r = new System.Random(DateTime.Now.Millisecond);

        QuestProcessor()
        {
            LoadAllQuests();
        }      

        void LoadAllQuests()
        {
            int i = 0;
            var txt = Resources.Load<TextAsset>("Quest" + i.ToString());
            while (txt != null)
            {
                LoadQuest(txt);
                i++;
                txt = Resources.Load<TextAsset>("Quest" + i.ToString());
            }
        }

        void LoadQuest(TextAsset txt)
        {
            var strings = txt.text.Split('\n');

            int LastQuestId = QuestCases.Count;

            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str) || str.Length < 4)
                    continue;
                var qp = new QuestPage(str);
                if (qp.IsStartPage)
                {
                    Quests.Add(qp);
                    //Добавляем запись в коллекцию квестовых диалогов
                    QuestCases.Add(new Dictionary<int, QuestPage>());
                }
                else
                {
                    //Если для нового квеста создана запись в коллекции диалогов
                    if (QuestCases.Count > LastQuestId)
                    {
                        if (!QuestCases[QuestCases.Count - 1].ContainsKey(qp.Id))
                            QuestCases[QuestCases.Count - 1].Add(qp.Id, qp);
                    }
                }
            }
        }

        /// <summary>
        /// Generates quest id or "no quest" result
        /// </summary>
        /// <param name="questId">Id of assigned quest</param>
        /// <returns>False if no quest</returns>
        public static bool GetRandomQuestId(ref int questId)
        {
            int questCount = Instance.Quests.Count;
            questId = r.Next(BalanceParameters.GetBalancedProbabilityValueForAssigningQuest(questCount));
            //questId = 1;
            return questId < questCount;
        }

        public static void FinishQuest()
        {
            GameManager.State = GameState.Space;
        }

        public static QuestPage GetQuest(int id)
        {
            if (Instance.Quests == null || Instance.Quests.Count <= id)
                return null;
            return Instance.Quests[id];
        }

        public static QuestPage GetQuestPage(int questId, int pageId)
        {
            if (Instance.QuestCases != null && Instance.QuestCases.Count > questId
                && Instance.QuestCases[questId] != null && Instance.QuestCases[questId].ContainsKey(pageId))
                return Instance.QuestCases[questId][pageId];
            return null;
        }
    }
}