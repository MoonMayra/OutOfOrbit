using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TrayectoryPreview : MonoBehaviour
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

        for (int i = 0; i < maxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, Mathf.Infinity, bouncesMask);
            if (hit.collider)
            {
                bouncesPoints.Add(hit.point);

                if (i == maxBounces)
                {
                    break;
                }
                currentDir = Bounce(currentDir, hit.normal);
                currentPos = hit.point + currentDir * 0.01f;
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

    public Vector2 Bounce(Vector2 direction, Vector2 normal)
    { 
        if (Mathf.Abs(normal.y)>0.9f) direction.y=-direction.y;
        if(Mathf.Abs(normal.x)>0.9f) direction.x=-direction.x;
        return direction.normalized;

    }

    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
    }

}
