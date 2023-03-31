using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestScript))]
public class QuestEditor : Editor
{
    private QuestScript _this;
    public override void OnInspectorGUI()
    {
        //set target for later use
        _this = (QuestScript)target;

        //Title
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 30f);
        GUILayout.Label("Quests");
        GUILayout.EndHorizontal();

        //Add New Quest Entries 
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width/3);
        if (GUILayout.Button("Add Quest",GUILayout.Width(Screen.width / 3)))
        {
            _this.Quests.Add(new Quest("", false));
        }
        GUILayout.Space(Screen.width / 3);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        //more titles
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(30f));
        GUILayout.Label("Name:");
        GUILayout.Label("Value:", GUILayout.Width(100f));
        GUILayout.EndHorizontal();
        for (var i = 0; i < _this.Quests.Count; i++)
        {
            GUILayout.BeginHorizontal();
            //delete button for quest
            if (GUILayout.Button("-", GUILayout.Width(25f),GUILayout.Height(15f)))
            {
                _this.Quests.RemoveAt(i);
            }
            //Quest Title - Editable
            _this.Quests[i].QuestName = GUILayout.TextField(_this.Quests[i].QuestName);
            //Quest Value Toggle
            if (GUILayout.Button(_this.Quests[i].QuestValue.ToString(), GUILayout.Width(100f) , GUILayout.Height(15f)))
            {
                _this.Quests[i].QuestValue = !_this.Quests[i].QuestValue;
            }
            GUILayout.EndHorizontal();
        }
    }
}
