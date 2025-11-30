using UnityEngine;
using System.Collections;

public class AmiController : MonoBehaviour
{
    //AMI
    public enum  AmiPath
    {
        Top_LtoR,
        Bottom_RtoL,
        Left_BtoT,
        Right_TtoB
    }
    [Header("Ami Path Points")]
    public Transform topLeft;
    public Transform topRight;

    public Transform bottomLeft;
    public Transform bottomRight;

    public Transform leftTop;
    public Transform leftBottom;

    public Transform rightTop;
    public Transform rightBottom;

    [Header("Other References")]
    [SerializeField] private LayerMask bulletMask;

    private Rigidbody2D rigidBody;

    //ARROWS

    public enum arrowDir
    {
        Right,
        Left,
        Up,
        Down
    }
    [Header("Arrows")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnTop;
    [SerializeField] private Transform spawnBottom;
    [SerializeField] private Transform spawnLeft;
    [SerializeField] private Transform spawnRight;

    private Animator arrowAnimator;
    private GameObject currentArrow;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentArrow = Instantiate(arrowPrefab);
        arrowAnimator = currentArrow.GetComponentInChildren<Animator>();
        currentArrow.SetActive(false);
    }
    private Transform GetSpawn(AmiPath path)
    {
        switch (path)
        {
            case AmiPath.Top_LtoR:
                return spawnTop;
            case AmiPath.Bottom_RtoL:
                return spawnBottom;
            case AmiPath.Left_BtoT:
                return spawnLeft;
            case AmiPath.Right_TtoB:
                return spawnRight;
        }
        return spawnTop;
    }
    private arrowDir GetDir(AmiPath path, bool isReturning)
    {
        switch (path)
        {
            case AmiPath.Top_LtoR:
                if (isReturning)
                {
                    return arrowDir.Left;
                }
                else
                {
                    return arrowDir.Right;
                }
            case AmiPath.Bottom_RtoL:
                if (isReturning)
                {
                    return arrowDir.Right;
                }
                else
                {
                    return arrowDir.Left;
                }
            case AmiPath.Left_BtoT:
                if (isReturning)
                {
                    return arrowDir.Down;
                }
                else
                {
                    return arrowDir.Up;
                }
            case AmiPath.Right_TtoB:
                if (isReturning)
                {
                    return arrowDir.Up;
                }
                else
                {
                    return arrowDir.Down;
                }
        }
        return arrowDir.Right;
    }

    public void ShowSign(AmiPath path)
    {
        Transform spawn = GetSpawn(path);
        arrowDir dir = GetDir(path, false);

        currentArrow.transform.position = spawn.position;
        currentArrow.SetActive(true);
        arrowAnimator.Play(dir.ToString());
    }
    public void ShowReturnSign(AmiPath path)
    {
        Transform spawn = GetSpawn(path);
        arrowDir dir = GetDir(path, true);

        currentArrow.transform.position = spawn.position;
        currentArrow.SetActive(true);
        arrowAnimator.Play(dir.ToString());
    }

    public void HideSign()
    {
        currentArrow.SetActive(false);
    }

    public void StartPath(AmiPath path, float duration)
    {
        switch (path)
        {
            case AmiPath.Top_LtoR:
                StartCoroutine(MoveAmi(topLeft.position, topRight.position, duration));
                break;
            case AmiPath.Bottom_RtoL:
                StartCoroutine(MoveAmi(bottomRight.position, bottomLeft.position, duration));
                break;
            case AmiPath.Left_BtoT:
                StartCoroutine(MoveAmi(leftBottom.position, leftTop.position, duration));
                break;
            case AmiPath.Right_TtoB:
                StartCoroutine(MoveAmi(rightTop.position, rightBottom.position, duration));
                break;

        }
    }
    public void ReturnToStart(AmiPath path, float duration)
    {
        switch (path)
        {
            case AmiPath.Top_LtoR:
                StartCoroutine(MoveAmi(topRight.position, topLeft.position, duration));
                break;
            case AmiPath.Bottom_RtoL:
                StartCoroutine(MoveAmi(bottomLeft.position, bottomRight.position, duration));
                break;
            case AmiPath.Left_BtoT:
                StartCoroutine(MoveAmi(leftTop.position, leftBottom.position, duration));
                break;
            case AmiPath.Right_TtoB:
                StartCoroutine(MoveAmi(rightBottom.position, rightTop.position, duration));
                break;
        }
    }
    private IEnumerator MoveAmi(Vector3 startPos, Vector3 endPos, float duration)
    {
        rigidBody.linearVelocity=Vector2.zero;
        rigidBody.position=startPos;

        float distance= Vector2.Distance(startPos,endPos);
        float speed= distance/duration;
        Vector2 direction= (endPos - startPos).normalized;

        rigidBody.linearVelocity= direction * speed;

        while (Vector2.Distance(rigidBody.position,endPos) > 0.1f)
        {
            yield return null;
        }
        rigidBody.linearVelocity=Vector2.zero;
        rigidBody.position=endPos;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1<<collision.gameObject.layer & bulletMask)!=0)
        {
            AmiFightManager.Instance.AmiHit();
            BulletMovement bulletMovement= collision.gameObject.GetComponent<BulletMovement>();
            bulletMovement.DestroyBulletsOnHazards();
        }
    }
    public void PhaseChange()
    {
        //Implementar cambio de fase visual
    }
}
