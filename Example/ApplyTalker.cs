using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTalker : MonoBehaviour
{
    public PlayerTalkScript Player;
    public NpcScript NPC;
    void Update()
    {
        if (!Player.Talker)
        {
            Player.Talker = NPC;
        }
    }
}
