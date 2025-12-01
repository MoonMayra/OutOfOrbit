using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Checkpoint currentCheckpoint;

    private Dictionary<string, bool> collectedItems = new Dictionary<string, bool>();

    [SerializeField] public PlayerManager player;
    [SerializeField] private PlayerStats playerStats;

    public string lastScenePlayed;

    public bool isNewGame = false;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        TryAssignPlayer();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Boot" ||
            scene.name == "MainMenu" ||
            scene.name == "AreaSelector" ||
            scene.name == "DebriefScreen")
            return;

        ResetLevelAndValues();

        lastScenePlayed = scene.name;

        PlayerPrefs.SetString("LastScenePlayed", lastScenePlayed);
        PlayerPrefs.Save();
    }



    private void TryAssignPlayer()
    {
        if (player == null)
            player = PlayerManager.Instance;

        if (player != null && playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    public void SetLastScene(string sceneName)
    {
        lastScenePlayed = sceneName;
    }

    public void RegisterCollectable(Collectable item)
    {
        if (!collectedItems.ContainsKey(item.id))
            collectedItems.Add(item.id, false);
    }

    public void ResetCollectablesState(Collectable item)
    {
        if (collectedItems.ContainsKey(item.id))
            collectedItems[item.id] = false;
    }

    public bool IsCollected(string id)
    {
        return collectedItems.ContainsKey(id) && collectedItems[id];
    }

    public void SetCollected(string id)
    {
        if (collectedItems.ContainsKey(id))
            collectedItems[id] = true;
    }

    public void RespawnPlayer(Transform spawnpoint)
    {
        TryAssignPlayer();

        if (player == null)
        {
            return;
        }

        if (currentCheckpoint == null)
        {
            return;
        }

        player.RespawnAt(spawnpoint.position);

        foreach (var collectable in FindObjectsByType<Collectable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (collectable.checkpointIndex == currentCheckpoint.index)
            {
                bool wasCollected = IsCollected(collectable.id);

                if (wasCollected)
                {
                    collectable.ResetCollectable();
                    playerStats.RemoveCollectable(1);
                    collectedItems[collectable.id] = false;
                }
            }
        }

        foreach (var hazard in FindObjectsByType<DropHazard>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Destroy(hazard.gameObject);
        }

        foreach (var platform in FindObjectsByType<MovementPlat>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            platform.ResetPlatform();
        }
        if(WaterBoss.Instance != null)
        {
            WaterBoss.Instance.hasDied = true;
        }
        foreach (var rock in FindObjectsByType<RockRestart>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            rock.RespawnRock();
        }
        foreach (var breakPlatform in FindObjectsByType<BreakablePlat>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            breakPlatform.ResetPlatform();
        }
        foreach (var wall in FindObjectsByType<BreakableWall>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            wall.ResetWall();
        if(AmiFightManager.Instance != null)
        {
            AmiFightManager.Instance.ResetFight();
        }
    }

    public void ResetLevelAndValues()
    {
        if (isNewGame)
        {
            PlayerStats.Instance.ResetValues();
            PlayerStats.Instance.ResetTimer();
            PlayerStats.Instance.ResetCollectibles();
            isNewGame = false;
        }

        PlayerStats.Instance.StartTimer();
    }



    public void LoadNextScene(string sceneName)
    {
        lastScenePlayed = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("LastScenePlayed", lastScenePlayed);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
