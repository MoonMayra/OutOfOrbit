using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public enum DialogueEndMode
    {
        None,
        LoadScene,
        EnableTrigger,
        DisableTrigger,
        CustomAction,
    }

    [Header("Dialogue Settings")]
    [SerializeField] Dialogue conversation;
    [SerializeField] DialogueManager manager;
    private bool alreadyTriggered = false;

    [Header("End of Dialogue Settings")]
    [SerializeField] private DialogueEndMode endMode = DialogueEndMode.None;

    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject triggerToEnable;
    [SerializeField] private GameObject triggerToDisable;
    
    public UnityEvent customAction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !alreadyTriggered)
        {
            alreadyTriggered = true;

            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.targetVelX = 0;
                PlayerMovement.Instance.moveInputX = 0;
                PlayerMovement.Instance.playerRigidBody.linearVelocity = Vector2.zero;
                PlayerMovement.Instance.enabled = false;
            }
            if (PlayerShoot.Instance != null)
            {
                PlayerShoot.Instance.enabled = false;
            }

            manager.GetConversation(conversation, OnDialogueFinished);

            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnDialogueFinished()
    {
        switch (endMode)
        {
            case DialogueEndMode.LoadScene:
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
                break;

            case DialogueEndMode.EnableTrigger:
                if (triggerToEnable != null)
                {
                    triggerToEnable.SetActive(true);
                }
                break;

            case DialogueEndMode.DisableTrigger:
                if (triggerToDisable != null)
                {
                    triggerToDisable.SetActive(false);
                }
                break;

            case DialogueEndMode.CustomAction:
                customAction.Invoke();
                break;

            case DialogueEndMode.None:
                break;
        }
    }

}
