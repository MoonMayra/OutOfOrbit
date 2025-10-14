using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool countFromStart = true;

    private float time;
    private bool active;

    private void Start()
    {
        if(countFromStart)
        {
            active = true;
        }
        else
        {
            active = false;
        }
        time = 0f;
    }

   private void Update()
    {
        if(!active) return;

        time += Time.deltaTime;

        int hours = (int)(time / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);

        timerText.text= string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
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
        timerText.text = "00:00:00";
    }
}
