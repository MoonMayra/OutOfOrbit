using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lineText;

    [SerializeField] GameObject dialoguePanel;

    [SerializeField] private Image panelImage;

    [SerializeField] private Color miaPruple = new Color(0.7f, 0.3f, 1f,0.4f);
    [SerializeField] private Color miaCyan = new Color(0f, 1f, 1f,0.4f);
    [SerializeField] private Color defaultColor = new Color(0f,0f,1f,0.4f);

    bool dialogueIsActive;
    public Action OnDialogueFinished;
    Queue<Line> lines = new Queue<Line>();

    public void Update()
    {
        if (dialogueIsActive && Input.GetKeyDown(KeyCode.Space))
        {
            ProduceNextLine();
        }
    }
    public void GetConversation(Dialogue conversation, Action onFinish = null)
    {
        OnDialogueFinished = onFinish;

        lines.Clear();
        foreach (var line in conversation.conversationLines)
        {
            lines.Enqueue(line);
        }

        dialogueIsActive = true;
        dialoguePanel.SetActive(true);
        ProduceNextLine();
    }

    void ProduceNextLine()
    {
        if (lines.Count == 0)
        {
            dialogueIsActive = false;
            dialoguePanel.SetActive(false);

            OnDialogueFinished?.Invoke(); 
            OnDialogueFinished = null;

            PlayerMovement.Instance.enabled = true;
            return;
        }
        Line currentLine = lines.Dequeue();

        if (currentLine.speakerName == "Mia")
        {
            panelImage.color = miaPruple;
        }
        else if (currentLine.speakerName == "Mia ")
        {
            panelImage.color = miaCyan;
        }
        else
        {
            panelImage.color = defaultColor;
        }

        nameText.text = currentLine.speakerName;
        lineText.text = currentLine.dialogueLine;
    }
}
