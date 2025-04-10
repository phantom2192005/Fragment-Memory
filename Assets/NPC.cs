using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialougeText, nameText;
    public Image potraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (Time.timeScale == 0 && !isDialogueActive)) 
        {
            return;
        }
        if (isDialogueActive) 
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }

    }
    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialougeText.SetText(dialogueData.dialogueLine[dialogueIndex]);
            isTyping = false;
        }
        else if(++ dialogueIndex < dialogueData.dialogueLine.Length) 
        {
            // if another line , type next line
            StartCoroutine(TypeLine());

        }
        else
        {
            EndDialouge();
        }
    }
    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        potraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        Time.timeScale = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialougeText.SetText("");

        foreach (char letter in dialogueData.dialogueLine[dialogueIndex])
        {
            dialougeText.text += letter;
            // Thay WaitForSeconds bằng WaitForSecondsRealtime
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }
        isTyping = false;

        // Xử lý tự động chuyển dòng (nếu có)
        if (dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            // Sử dụng Realtime ở đây nữa
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
    public void EndDialouge()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialougeText.SetText("");
        dialoguePanel.SetActive(false);
        Time.timeScale = 1;

    }
}
