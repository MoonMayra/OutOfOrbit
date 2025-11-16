using TMPro;
using UnityEngine;

public class DeathDebrief : MonoBehaviour
{
    private PlayerStats playerStats;
    private TMP_Text deathText;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        deathText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        deathText.text = playerStats.deaths.ToString();
    }
}
