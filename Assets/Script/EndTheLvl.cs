using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTheLvl : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private string nextScene;   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
