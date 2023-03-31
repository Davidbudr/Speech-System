using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class NpcScript : MonoBehaviour
{
    public string NpcName;
    public Sprite NpcSprite;
    
    [SerializeField]
    public List<Page> Pages = new List<Page>();

    public int findPage(string name)
    {
        if (name == "Exit")
        {
            return -1;
        }
        else{
            for (var i = 0; i < Pages.Count; i++)
            {
                if (name == Pages[i].PageID)
                {
                    return i;
                }
            }
        }
        return -1;
    }
}

[Serializable]
public class Page
{
    public string PageID;
    public Sprite Image;
    public string Description;
    [SerializeField]
    public List<ReplyOptions> Options = new List<ReplyOptions>();

    public Page() { }
    public Page(string name)
    {
        PageID = name;
    }
}

[Serializable]
public class ReplyOptions
{
    public string PageLink;
    public string LinkText;
    public List<Quest> Requirement = new List<Quest>();
    public List<Quest> Rewards = new List<Quest>();
    [SerializeField]
    public UnityEvent myEvent;
}
