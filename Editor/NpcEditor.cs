using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class NpcEditor : EditorWindow
{
    //custom editor for Npc Script
    [MenuItem("Quest Editor/Npc Editor")]
    static void Init()
    {
        NpcEditor window = (NpcEditor)EditorWindow.GetWindow(typeof(NpcEditor));
        window.Show();
    }
    public NpcScript target;
    public int OpenWindow;
    private Vector2 vScroll;
    private Vector2 v2Scroll;

    private void OnGUI()
    {
        //set target to selected gameobject
        if (Selection.activeGameObject != target)
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.GetComponent<NpcScript>() != null)
                {
                    target = Selection.activeGameObject.GetComponent<NpcScript>();
                }
                else
                {
                    target = null;
                }
                Repaint();
            }
        }

        if (target)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(target.NpcName);
            //save the current scene because it doesnt keep save otherwise
            if (GUILayout.Button("Save", GUILayout.Width(100f)))
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            //left most area, for Page buttons
            GUILayout.BeginVertical(GUILayout.Width(100f));
            vScroll = GUILayout.BeginScrollView(vScroll, GUILayout.Width(100f));
            if (GUILayout.Button("Settings"))
            {
                OpenWindow = -1;
            }
            //add all the pages on the target as tabs
            for (var i = 0; i < target.Pages.Count; i++)
            {
                if (GUILayout.Button(target.Pages[i].PageID))
                {
                    OpenWindow = i;
                }
            }
            GUILayout.FlexibleSpace();
            //add new pages to the target
            if (GUILayout.Button("Add Page"))
            {
                target.Pages.Add(new Page("Page" + (target.Pages.Count + 1)));
            }
            //delete currently selected page
            if (OpenWindow >= 0)
            {
                if (GUILayout.Button("Remove Page"))
                {
                    target.Pages.RemoveAt(OpenWindow);
                    OpenWindow = -1;
                }
            }

            //end of left most area
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            //begin right most area
            if (OpenWindow == -1) // npc settings
            {
                GUILayout.BeginVertical();

                //Npc name setting
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name: ", GUILayout.Width(80f));
                target.NpcName = EditorGUILayout.TextField(target.NpcName);
                GUILayout.EndHorizontal();

                //Default sprite to use if a new sprite isnt used in the page
                GUILayout.BeginHorizontal();
                GUILayout.Label("Default Sprite: ");
                target.NpcSprite = (Sprite)EditorGUILayout.ObjectField(target.NpcSprite, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(100f));
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();

                //Delete all the pages from this npc
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Clear Npc", GUILayout.Width(100f)))
                {
                    target.Pages.Clear();
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            else //page settings
            {
                //begin the right most area
                GUILayout.BeginVertical();
                v2Scroll = GUILayout.BeginScrollView(v2Scroll);

                //Give the page a name
                GUILayout.BeginHorizontal();
                GUILayout.Label("PageID: ", GUILayout.Width(80f));
                target.Pages[OpenWindow].PageID = GUILayout.TextField(target.Pages[OpenWindow].PageID);
                GUILayout.EndHorizontal();

                //sprite for current page
                GUILayout.BeginHorizontal();
                GUILayout.Label("Page Sprite: ");
                target.Pages[OpenWindow].Image = (Sprite)EditorGUILayout.ObjectField(target.Pages[OpenWindow].Image, typeof(Sprite), false, GUILayout.Width(100f), GUILayout.Height(100f));
                GUILayout.EndHorizontal();

                //current pages speech
                GUILayout.BeginHorizontal();
                GUILayout.Label("Page speech: ", GUILayout.Width(80f));
                target.Pages[OpenWindow].Description = GUILayout.TextArea(target.Pages[OpenWindow].Description, GUILayout.MinHeight(100f));
                GUILayout.EndHorizontal();

                //add new replies
                GUILayout.BeginHorizontal();
                GUILayout.Label("Replies: ");
                if (GUILayout.Button("Add Reply +", GUILayout.MaxWidth(200f)))
                {
                    target.Pages[OpenWindow].Options.Add(new ReplyOptions());
                }
                GUILayout.EndHorizontal();

                //reply titles
                GUILayout.BeginHorizontal();
                GUILayout.Label("Goto ", GUILayout.Width(100f));
                GUILayout.Label("Reply Text ");
                GUILayout.EndHorizontal();

                //display each reply
                for (var i = 0; i < target.Pages[OpenWindow].Options.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(target.Pages[OpenWindow].Options[i].PageLink, GUILayout.Width(100f)))
                    {
                        //show a window to select the destination of the reply
                        selectorWindow mysel = (selectorWindow)EditorWindow.GetWindow(typeof(selectorWindow));
                        mysel.Target = target;
                        mysel.Page = OpenWindow;
                        mysel.Option = i;
                        mysel.Show();
                    }

                    //text of the current reply
                    target.Pages[OpenWindow].Options[i].LinkText = GUILayout.TextField(target.Pages[OpenWindow].Options[i].LinkText, GUILayout.MinWidth(100f));

                    if (GUILayout.Button("Rewards", GUILayout.Width(100f)))
                    {
                        //show a window to add rewards for the reply
                        RewardWindow myq = (RewardWindow)EditorWindow.GetWindow(typeof(RewardWindow));
                        myq.Target = target;
                        myq.Page = OpenWindow;
                        myq.Option = i;
                        myq.Show();
                    }

                    if (GUILayout.Button((target.Pages[OpenWindow].Options[i].Requirement.Count > 0) ? "Requirement" : "No Requirement", GUILayout.Width(150f)))
                    {
                        //show a window to add requirements for the reply to display
                        QuestWindow myq = (QuestWindow)EditorWindow.GetWindow(typeof(QuestWindow));
                        myq.Target = target;
                        myq.Page = OpenWindow;
                        myq.Option = i;
                        myq.Reward = false;
                        myq.Show();
                    }

                    //button to delete the current reply
                    if (GUILayout.Button("-", GUILayout.Width(30f)))
                    {
                        target.Pages[OpenWindow].Options.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                //end the right most area
            }


            GUILayout.EndHorizontal();

            //display warning message at the bottom of the screen
            GUILayout.Label("WARNING: Exiting the scene without saving will cause all progress to be lost");
        }
        else
        {
            // if nothing is selected display message
            GUILayout.Label("Theres no npc selected");
        }
    }
}

public class RewardWindow : EditorWindow
{
    public NpcScript Target;
    public int Page;
    public int Option;
    public Vector2 Scroll;
    private void OnGUI()
    {
        //Title
        GUILayout.Label("Rewards");

        //Unity Events to run when the option is selected
        var so = new SerializedObject(Target);
        so.Update();
        var sp = so.FindProperty("Pages").GetArrayElementAtIndex(Page).FindPropertyRelative("Options").GetArrayElementAtIndex(Option).FindPropertyRelative("myEvent");
        EditorGUILayout.PropertyField(sp);
        so.ApplyModifiedProperties();

    }

}
public class QuestWindow : EditorWindow
{
    public NpcScript Target;
    public int Page;
    public int Option;
    public bool Reward;
    public Vector2 Scroll;

    //quest selector for rewards or requirements
    private void OnGUI()
    {
        //select a label based on bool
        GUILayout.Label((Reward) ? "Rewards" : "Requirements");

        //based on the same bool add quest item to reply
        if (GUILayout.Button((Reward) ? "Add Reward" : "Add Requirement"))
        {
            if (Reward)
            {
                Target.Pages[Page].Options[Option].Rewards.Add(new Quest("", false));
            }
            else
            {
                Target.Pages[Page].Options[Option].Requirement.Add(new Quest("", false));
            }
        }

        if (Reward)
        {
            //display each of the reward quests and what value they are set to
            for (var i = 0; i < Target.Pages[Page].Options[Option].Rewards.Count; i++)
            {
                GUILayout.BeginHorizontal();
                Target.Pages[Page].Options[Option].Rewards[i].QuestName = EditorGUILayout.TextField(Target.Pages[Page].Options[Option].Rewards[i].QuestName);
                Target.Pages[Page].Options[Option].Rewards[i].QuestValue = EditorGUILayout.Toggle(Target.Pages[Page].Options[Option].Rewards[i].QuestValue, GUILayout.Width(50f));

                //delete quest from list
                if (GUILayout.Button("-", GUILayout.Width(30f)))
                {
                    Target.Pages[Page].Options[Option].Rewards.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();
            }


        }
        else
        {
            //display each of the requirement quests and what value they need
            for (var i = 0; i < Target.Pages[Page].Options[Option].Requirement.Count; i++)
            {
                GUILayout.BeginHorizontal();
                Target.Pages[Page].Options[Option].Requirement[i].QuestName = EditorGUILayout.TextField(Target.Pages[Page].Options[Option].Requirement[i].QuestName);
                Target.Pages[Page].Options[Option].Requirement[i].QuestValue = EditorGUILayout.Toggle(Target.Pages[Page].Options[Option].Requirement[i].QuestValue, GUILayout.Width(50f));
                //delete quest from list
                if (GUILayout.Button("-", GUILayout.Width(30f)))
                {
                    Target.Pages[Page].Options[Option].Requirement.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();
            }
        }

    }
}
public class selectorWindow : EditorWindow
{
    public NpcScript Target;
    public int Page;
    public int Option;
    public Vector2 Scroll;

    private void OnGUI()
    {
        //display each page as a button along with an exit option
        Scroll = GUILayout.BeginScrollView(Scroll);
        for (var i = 0; i < Target.Pages.Count; i++)
        {
            if (GUILayout.Button(Target.Pages[i].PageID))
            {
                //set the page link and close the window
                Target.Pages[Page].Options[Option].PageLink = Target.Pages[i].PageID;
                EditorWindow.GetWindow(typeof(selectorWindow)).Close();
            }
        }
        if (GUILayout.Button("Exit"))
        {
            //set the exit link and close the window
            Target.Pages[Page].Options[Option].PageLink = "Exit";
            EditorWindow.GetWindow(typeof(selectorWindow)).Close();
        }
        GUILayout.EndScrollView();
    }
}

[CustomEditor(typeof(NpcScript))]
public class NpcExtend : Editor
{
    public NpcScript _this;
    public override void OnInspectorGUI()
    {
        //In the inspector only allow npc name editing
        _this = (NpcScript)target;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Npc Name: ");
        _this.NpcName = GUILayout.TextField(_this.NpcName);
        GUILayout.EndHorizontal();

        //allow for opening of the npc editor from here
        if (GUILayout.Button("Open Editor"))
        {
            NpcEditor window = (NpcEditor)EditorWindow.GetWindow(typeof(NpcEditor));
            window.Show();
        }
    }
}
