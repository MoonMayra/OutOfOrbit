using UnityEngine;

public class CameraLimits : MonoBehaviour
{
    public float minX = 0;
    public float maxX = 0;
    public float minY = 0;
    public float maxY = 0;
    public bool clampX = true;
    public bool clampY = true;
    public bool stopX = false;
    public bool stopY = false;

    private Vector3 startPos;

    private void Start()
    {
        startPos= transform.position;
    }
    public void ClampYAxes()
    {
               Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
    public void ClampXAxes()
    {
               Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
    public void StopYAxes()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, startPos.y, startPos.y);
        transform.position = pos;
    }
    public void StopXAxes()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, startPos.x, startPos.x);
        transform.position = pos;
    }
    private void LateUpdate()
    {
        if(clampX)
        {
            ClampXAxes();
        }
        if(clampY)
        {
            ClampYAxes();
        }
        if(stopX)
        {
            StopXAxes();
        }
        if(stopY)
        {
            StopYAxes();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }

}
