using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;
using System;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI rewards;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardText;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            ClearObjectives();
            foreach(var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (status.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }
            rewards.text = GetRewardText(quest);
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";

            foreach(var rewards in quest.GetRewards())
            {
                if(rewardText != "")
                {
                    rewardText += ", ";
                }
                if(rewards.number > 1)
                {
                    rewardText += rewards.number + " ";
                }
                rewardText += rewards.item.GetDisplayName();
            }

            if(rewardText == "")
            {
                rewardText = "No reward";
            }
            rewardText += ".";

            return rewardText;
        }

        private void ClearObjectives()
        {
            foreach(Transform child in objectiveContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
