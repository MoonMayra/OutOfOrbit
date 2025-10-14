using UnityEngine;
using UnityEngine.SceneManagement;

public class Drown : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        playerStats = GameObject.Find(playerTag).GetComponent<PlayerStats>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerStats.AddDeath(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerStats.AddDeath(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
