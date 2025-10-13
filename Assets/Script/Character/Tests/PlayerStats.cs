using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int deaths = 0;
    public int collectables = 0;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public void AddCollectable(int amount)
    {
        collectables += amount;
    }

    public void AddDeath(int amount)
    {
        deaths += amount;
    }

    public void RemoveCollectable(int amount)
    {
        collectables -= amount;
    }
    public void ResetValues()
    {
        deaths = 0;
        collectables = 0;
    }
}
