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
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
