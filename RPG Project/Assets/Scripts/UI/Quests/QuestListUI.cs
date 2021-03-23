using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using System;

public class QuestListUI : MonoBehaviour
{
    
    [SerializeField] QuestItemUI questItemPrefab;
    QuestList questList;

    private void Start()
    {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.onUpdate += Redraw;
        Redraw();
    }

    void Redraw()
    {
        ClearQuests();

        if(questList.GetStatuses() == null)
        {
            return;
        }
        foreach(QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI instance = Instantiate<QuestItemUI>(questItemPrefab, transform);
            instance.Setup(status);
        }
    }

    private void ClearQuests()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
