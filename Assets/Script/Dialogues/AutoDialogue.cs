using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoDialogue : MonoBehaviour
{
    public enum DialogueEndMode
    {
        None,
        LoadScene,
    }

    [Header("Dialogue Settings")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("End Behaviour")]
    [SerializeField] private DialogueEndMode endMode = DialogueEndMode.None;
    [SerializeField] private string sceneToLoad;

    private void Start()
    {
        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueManager.GetConversation(dialogue, OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        switch (endMode)
        {
            case DialogueEndMode.LoadScene:
                if (!string.IsNullOrEmpty(sceneToLoad))
                    SceneManager.LoadScene(sceneToLoad);
                break;

            case DialogueEndMode.None:
                break;
        }
    }
}
