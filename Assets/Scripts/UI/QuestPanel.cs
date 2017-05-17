using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Catopus.Quest;

namespace Catopus.UI
{
    public class QuestPanel : MonoBehaviour
    {
        static System.Random r = new System.Random();
        //[SerializeField]
        //QuestDialogueOption[] Options = new QuestDialogueOption[5];

        [SerializeField]
        Text NPCText;
        [SerializeField]
        Text[] AnswerTexts = new Text[5];
        [SerializeField]
        GameObject[] AnswerButtons = new GameObject[5];

        int currentQuestId;
        QuestPage[] answers = new QuestPage[5];
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowQuest(int questId)
        {
            QuestPage q = QuestProcessor.GetQuest(questId);
            if (q == null)
                return;

            currentQuestId = questId;
            if (NPCText != null)
                NPCText.text = q.Text;
            if (q.Cases == null)
                return;
            FillAnswers(q.Cases);
        }

        public void ShowNextQuestPage(int answerId)
        {
            var b = AnswerButtons[answerId].GetComponent<Button>();
            b.GetComponent<Image>().color = b.colors.normalColor;


            var a = answers[answerId];
            /*
            if (a == null)
                return;
            if (a.Cases == null || a.Cases.Count == 0)
                return;
            int caseId = a.Cases[r.Next(a.Cases.Count)];

            var q = QuestProcessor.GetQuestPage(currentQuestId, caseId);
            */
            //TODO stupid to kep all answers as quest pages and than use only ids
            var q = QuestProcessor.GetAnswerResult(currentQuestId, answers[answerId].Id);
            if (q.IsEndPage)
            {
                var reward = BalanceParameters.GetBalancedReward(q.Reward);
                PlayerController.Instance.ApplyReward(reward);
                UIController.ShowQuestResult(reward, q.Text);
                gameObject.SetActive(false);
                return;
            }

            if (NPCText != null)
                NPCText.text = q.Text;
            FillAnswers(q.Cases);
        }

        void FillAnswers(List<int> answerIds)
        {
            //Debug.Log(answerIds.Count);
            for (int i = 0; i < 5; i++)
            {
                if (answerIds.Count > i)
                {
                    answers[i] = QuestProcessor.GetQuestPage(currentQuestId, answerIds[i]);
                    if (AnswerButtons[i] != null)
                    {
                        if (!AnswerButtons[i].activeSelf)
                            AnswerButtons[i].SetActive(true);
                    }
                    if (AnswerTexts[i] != null)
                    {
                        //Debug.Log(answers[i]);
                        AnswerTexts[i].text = answers[i] == null ? string.Empty : answers[i].Text;
                    }
                }
                else
                {
                    answers[i] = null;
                    if (AnswerButtons[i] != null)
                        AnswerButtons[i].SetActive(false);
                }
            }
        }
    }
}