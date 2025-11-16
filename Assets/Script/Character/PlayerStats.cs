using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int deaths = 0;
    public int collectables = 0;

    [SerializeField] public string timerText;
    [SerializeField] private bool countFromStart = false;

    public float time= 0f;
    private bool active;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        active = countFromStart;

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

    private void Update()
    {
        if (!active) return;

        time += Time.deltaTime;

        int hours = (int)(time / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);

        timerText = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    public void StartTimer()
    {
        active = true;
    }
    public void StopTimer()
    {
        active = false;
    }
    public void ResetTimer()
    {
        time = 0f;
        timerText= "00:00:00";
    }
}
