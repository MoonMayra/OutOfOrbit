using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Death : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Checkpoint current;
    private GlitchController glitchController;
    private Animator animatorGlitch;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        levelManager = LevelManager.Instance;
        glitchController=GlitchController.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            Debug.Log("Player collision" + collision);
            HandleDeath();

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            HandleDeath();
        }
    }
    private bool IsPlayer(GameObject obj)
    {
        return (((1 << obj.gameObject.layer) & player.value) != 0);
    }
    private void HandleDeath()
    {
        
        glitchController.GlitchDeath();
        if(playerStats != null) 
            playerStats.AddDeath(1);
        if (levelManager != null && levelManager.currentCheckpoint != null)
        {
            levelManager.RespawnPlayer(levelManager.currentCheckpoint.spawnPoint);
            Debug.Log("Player Respawned at Checkpoint: " + levelManager.currentCheckpoint.index);
        }
        else
        {
            Debug.Log("No current Checkpoint assigned");

        }

        
    }
}
