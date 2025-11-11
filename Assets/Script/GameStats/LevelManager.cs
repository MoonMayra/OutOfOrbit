using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Checkpoint currentCheckpoint;

    private Dictionary<string, bool> collectedItems= new Dictionary<string, bool>();

    [SerializeField] public PlayerManager player;
    private PlayerStats playerStats;


    private void Awake()
    {
        if (Instance==null)
            Instance=this;
        else
            Destroy(gameObject);

    }
    private void Start()
    {
        player = PlayerManager.Instance;
        playerStats=GetComponentInParent<PlayerStats>();
    }
    public void RegisterCollectable(Collectable item)
    {
        if(!collectedItems.ContainsKey(item.id))
        {
            collectedItems[item.id] = false;
        }
    }

    public void ResetCollectablesState(Collectable item)
    {
        collectedItems[item.id] = false;
        bool test= IsCollected(item.id);
    }
    public bool IsCollected(string id)
    {
        return collectedItems.ContainsKey(id) && collectedItems[id];
    }

    public void SetCollected(string id)
    {
        if(collectedItems.ContainsKey(id))
        {
            collectedItems[id] = true;
        }
    }

    public void RespawnPlayer(Transform spawnpoint)
    {
        player.RespawnAt(spawnpoint.position);

        foreach (var collectable in FindObjectsByType<Collectable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (collectable.checkpointIndex == currentCheckpoint.index)
            {
                bool wasCollected = IsCollected(collectable.id);

                if (wasCollected)
                {
                    collectable.ResetCollectable();
                    GetComponent<PlayerStats>().AddCollectable(-1);
                    collectedItems[collectable.id] = false;
                }
            }
        }
    }
}
