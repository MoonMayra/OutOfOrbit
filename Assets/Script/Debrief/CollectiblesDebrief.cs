using TMPro;
using UnityEngine;

public class CollectiblesDebrief : MonoBehaviour
{
    private PlayerStats playerStats;
    private TMP_Text collectiblesText;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        collectiblesText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        collectiblesText.text = playerStats.collectables.ToString();
    }
}
