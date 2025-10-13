using UnityEngine;
using UnityEngine.SceneManagement;

public class Drown : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            Debug.Log("You died!");
            playerStats.AddDeath(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
