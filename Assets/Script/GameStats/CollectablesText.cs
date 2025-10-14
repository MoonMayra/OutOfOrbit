using UnityEngine;
using TMPro;

public class CollectablesText : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI collectablesText;

    void Start()
    {
        collectablesText.text = "x " + playerStats.collectables.ToString();
    }

    void Update()
    {
        collectablesText.text = "x " + playerStats.collectables.ToString();
    }
}
