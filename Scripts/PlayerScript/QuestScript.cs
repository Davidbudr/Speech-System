using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestScript : MonoBehaviour
{
    [SerializeField]
    public List<Quest> Quests = new List<Quest>();

    public int GetQuest(string name)
    {
        for (var i = 0; i < Quests.Count; i++)
        {
            if (name == Quests[i].QuestName)
            {
                return i;
            }
        }
        return -1;
    }

    public void UpdateQuest(string name, bool value)
    {
        bool updated = false;
        for (var i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].QuestName == name)
            {
                updated = true;
                Quests[i].QuestValue = value;
            }
        }
        if (!updated)
        {
            Quests.Add(new Quest(name, value));
        }
    }

}
[Serializable]
public class Quest
{
    public string QuestName;
    public bool QuestValue;

    public Quest(string name, bool value)
    {
        QuestName = name;
        QuestValue = value;
    }
}
