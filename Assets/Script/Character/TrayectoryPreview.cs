using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TrayectoryPreview : MonoBehaviour
{
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private LayerMask bouncesMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private string platformsTag = "PassThrough";
    private PlayerMovement playerMovement;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }

    public void DrawTrayectory(Vector2 startPos, Vector2 lineDirection)
    {
        List<Vector3> bouncesPoints=new List<Vector3>();
        bouncesPoints.Add(startPos);

        Vector2 currentPos = startPos;
        Vector2 currentDir=lineDirection.normalized;

        for (int i = 0; i < maxBounces; i++)
        {
            LayerMask combinedMask = bouncesMask | bulletMask;
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, Mathf.Infinity, combinedMask);
            if (hit.collider)
            {
                if(hit.collider.CompareTag(platformsTag))
                {
                    currentPos=hit.point + currentDir * 0.01f;
                    i--;
                    continue;
                }

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
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            direction.x = -direction.x;
        }
        else
        {
            direction.y = -direction.y;
        }

        return direction.normalized;

    }

    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
    }

}
