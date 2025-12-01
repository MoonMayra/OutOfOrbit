using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue conversation;
    [SerializeField] DialogueManager manager;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !alreadyTriggered)
        {
            alreadyTriggered = true;

            manager.GetConversation(conversation);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
