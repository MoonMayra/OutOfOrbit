using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    private PlayerStats playerStats;
    private TMP_Text timerText;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        timerText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        timerText.text = playerStats.timerText;
    }
}
