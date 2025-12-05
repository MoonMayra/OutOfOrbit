using UnityEngine;

public class FloatAnim : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float initialY;

    private Vector3 startPos;

    private void Start()
    {
        initialY = transform.localPosition.y;
    }

    private void Update()
    {
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency) * amplitude;

        Vector3 localPos = transform.localPosition;
        localPos.y = initialY + yOffset;
        transform.localPosition = localPos;
    }
}
