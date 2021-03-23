using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

public class QuestItemUI : MonoBehaviour
{
    QuestStatus questStatus;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

   public void Setup(QuestStatus status)
    {
        questStatus = status;
        title.text = questStatus.GetQuest().GetTitle();
        progress.text = status.GetCompletedCount() + "/" + questStatus.GetQuest().GetObjectiveCount();
    }

    public QuestStatus GetQuestStatus()
    {
        return questStatus;
    }
}
