using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPC Dialouge" )]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLine;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1.0f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

}
