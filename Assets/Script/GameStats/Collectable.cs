using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string id;
    public int checkpointIndex;

    private void Start()
    {
        LevelManager.Instance.RegisterCollectable(this);
        
        if (LevelManager.Instance.IsCollected(id))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnCollected()
    {
        LevelManager.Instance.SetCollected(id);
        gameObject.SetActive(false);
    }

    public void ResetCollectable()
    {
        LevelManager.Instance.RegisterCollectable(this);
        gameObject.SetActive(true);
    }
}
