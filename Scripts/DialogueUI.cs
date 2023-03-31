using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public QuestScript Player;
    [HideInInspector]
    public NpcScript Talker;
    public GameObject DialogueCanvas;
    public GameObject ReplyCanvas;
    public Text NpcDialogue;
    public Image NpcImage;
    public List<Button> Replies = new List<Button>();
    
    private string sTalk;
    private string sCurTalk;
    private int iCurPage;
    private bool bTalked;

    public float Delay;
    private float ftimer;

    void Update()
    {

        NpcDialogue.text = sCurTalk;

        if (DialogueCanvas.activeSelf && sCurTalk == sTalk && Talker != null)
        {
            if (Talker.Pages[iCurPage].Options.Count > 0 && !bTalked)
            {
                bTalked = true;
                ReplyCanvas.SetActive(true);
                for (var i = 0; i < Talker.Pages[iCurPage].Options.Count; i++)
                {
                    if (Talker.Pages[iCurPage].Options[i].Requirement.Count > 0)
                    {
                        bool _enable = true;

                        for (var j = 0; j < Talker.Pages[iCurPage].Options[i].Requirement.Count; j++)
                        {
                            int _id = Player.GetQuest(Talker.Pages[iCurPage].Options[i].Requirement[j].QuestName);
                            if (_id == -1 || Player.Quests[_id].QuestValue != Talker.Pages[iCurPage].Options[i].Requirement[j].QuestValue)
                            {
                                _enable = false;
                                break;
                            }
                        }

                        if (_enable)
                        {
                            EnableButton(i);
                        }
                    }
                    else
                    {
                        EnableButton(i);
                    }

                }
            }
            else if (Talker.Pages[iCurPage].Options.Count <= 0)
            {
                ftimer += Time.deltaTime;
                if (ftimer > Delay)
                {
                    ftimer = 0;
                    if (Input.anyKey)
                    {
                        if (Talker.Pages.Count > iCurPage + 1)
                        {
                            BeginTalk(iCurPage + 1);
                        }
                        else
                        {
                            DisableDialogue();
                        }
                    }
                }
            }
        }
        else if (DialogueCanvas.activeSelf && Talker != null)
        {
            if (Input.anyKey)
            {
                if (sCurTalk != sTalk)
                {
                    sCurTalk += sTalk[sCurTalk.Length];
                }
            }
        }
    }
    void EnableButton(int OptionID)
    {
        Replies[OptionID].gameObject.SetActive(true);
        Replies[OptionID].transform.GetChild(0).GetComponent<Text>().text = Talker.Pages[iCurPage].Options[OptionID].LinkText;
        int _link = Talker.findPage(Talker.Pages[iCurPage].Options[OptionID].PageLink);

        if (_link >= 0)
        {
            Replies[OptionID].onClick.AddListener(delegate { BeginTalk(_link); });
        }
        else
        {
            Replies[OptionID].onClick.AddListener(delegate { DisableDialogue(); });
        }
        UnityEvent _reward = Talker.Pages[iCurPage].Options[OptionID].myEvent;
        Replies[OptionID].onClick.AddListener(delegate { _reward.Invoke(); print("INVOKED"); });
        for (var i = 0; i < Talker.Pages[iCurPage].Options[OptionID].Rewards.Count; i++)
        {
            //Quest _reward = Talker.Pages[iCurPage].Options[OptionID].Rewards[i];
            //Replies[OptionID].onClick.AddListener(delegate { RewardQuest(_reward.QuestName, _reward.QuestValue); });
        }

    }
    public void DisableDialogue()
    {
        Talker = null;
        DialogueCanvas.SetActive(false);
    }
    public void RewardQuest(string name, bool value)
    {
        int _questid = Player.GetQuest(name);

        if (_questid != -1)
        {
            Player.Quests[_questid].QuestValue = value;
        }
        else
        {
            Player.Quests.Add(new Quest(name, value));
        }
    }
    public void BeginTalk(int page)
    {
        if (page >= 0 && Talker.Pages.Count >= page)
        {
            ReplyCanvas.SetActive(false);
            bTalked = false;
            ftimer = 0;
            NpcImage.gameObject.SetActive(true);
            if (Talker.Pages[page].Image != null)
            {
                NpcImage.sprite = Talker.Pages[page].Image;
            }
            else if (Talker.NpcSprite != null)
            {
                NpcImage.sprite = Talker.NpcSprite;
            }
            else
            {
                NpcImage.gameObject.SetActive(false);
            }
            CancelInvoke("ChatWithMe");
            DialogueCanvas.SetActive(true);
            for (var i = 0; i < Replies.Count; i++)
            {
                Replies[i].gameObject.SetActive(false);
                Replies[i].onClick.RemoveAllListeners();
            }
            sTalk = Talker.Pages[page].Description;
            iCurPage = page;
            sCurTalk = "";
            bTalked = false;
            InvokeRepeating("ChatWithMe", 0, 0.1f);
        }
        else
        {
            Debug.LogError("This is an Exit!");
        }
    }
    void ChatWithMe()
    {
        if (sCurTalk != sTalk)
        {
            sCurTalk += sTalk[sCurTalk.Length];
        }
    }
}
