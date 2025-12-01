using UnityEngine;

public class StartFight : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            AmiController.Instance.startedFight = true;
            gameObject.SetActive(false);
        }
    }
}
