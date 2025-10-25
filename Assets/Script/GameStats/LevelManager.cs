using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Checkpoint currentCheckpoint;

    private Dictionary<string, bool> collectedItems= new Dictionary<string, bool>();

    [SerializeField] private PlayerManager player;

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
    }
    public void RegisterCollectable(Collectable item)
    {
        if(!collectedItems.ContainsKey(item.id))
        {
            collectedItems[item.id] = false;
        }
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

        foreach (var collectable in FindObjectsByType<Collectable>(FindObjectsSortMode.None)) 
        {
            if(collectable.checkpointIndex > currentCheckpoint.index)
            {
                collectable.ResetCollectable();
            }
        }
    }
}
