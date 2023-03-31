using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkScript : MonoBehaviour
{
    public NpcScript Talker;
    public DialogueUI DialogueInterface;

    void Update()
    {
        if (DialogueInterface.Talker == null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DialogueInterface.Talker = Talker;
                DialogueInterface.BeginTalk(0);
            }

        }
    }
}
