using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get;private set; }

    [SerializeField] private float defaultDuration = 0.15f;
    [SerializeField] private float defaultMagnitude = 0.2f;

    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        originalPos = transform.position;
    }
    public void Shake(float duration = -1, float magnitude = -1)
    {
        if (duration <= 0)
        {
            duration = defaultDuration;
        }
        if (magnitude <= 0)
        {
            magnitude = defaultMagnitude;
        }
        if(shakeRoutine!=null)
        {
            StopCoroutine(shakeRoutine);
        }
        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }
    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }



}
