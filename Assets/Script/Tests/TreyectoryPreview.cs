using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TreyectoryPreview : MonoBehaviour
{
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private LayerMask bouncesMask;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrayectory(Vector2 startPos, Vector2 lineDirection)
    {
        List<Vector3> bouncesPoints=new List<Vector3>();
        bouncesPoints.Add(startPos);

        Vector2 currentPos = startPos;
        Vector2 currentDir=lineDirection.normalized;

        for (int i = 0; i <= maxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, Mathf.Infinity, bouncesMask);
            if (hit.collider)
            {
                bouncesPoints.Add(hit.point);

                if (i == maxBounces)
                {
                    break;
                }
                currentDir = Vector2.Reflect(-currentDir, hit.normal);
            }
            else
            {
                bouncesPoints.Add(currentPos + currentDir * 1000f);
                break;
            }
        }
        lineRenderer.positionCount=bouncesPoints.Count;
        lineRenderer.SetPositions(bouncesPoints.ToArray());


    }

    private void Bounce(float direction, float normal)

    { }

    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
    }

}
