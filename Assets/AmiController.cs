using UnityEngine;
using System.Collections;

public class AmiController : MonoBehaviour
{
    public static AmiController Instance {  get; private set; }
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

    [Header("Cutscenes component")]
    [SerializeField] private Transform cutscenePoint;
    [SerializeField] private Transform originalPoint;
    [SerializeField] private Transform endPoint;

    [Header("Ami Parameters Phase 1")]
    [SerializeField] private float amiHitStunDurationP1 = 0.05f;
    [SerializeField] private float amiEnterTimeP1 = 3f;
    [SerializeField] private float amiStayTimeP1 = 2f;
    [SerializeField] private float amiExitTimeP1 = 3f;

    [Header("Ami Parameters Phase 2")]
    [SerializeField] private float amiHitStunDurationP2 = 0.05f;
    [SerializeField] private float amiEnterTimeP2 = 3f;
    [SerializeField] private float amiStayTimeP2 = 2f;
    [SerializeField] private float amiExitTimeP2 = 1f;

    private AmiView amiView;
    private Rigidbody2D rigidBody;
    private Coroutine currentMovementCoroutine;
    private Coroutine movingCoroutine;
    public bool startedFight = false;

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
        if(Instance==null)
        {
            Instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
        rigidBody = GetComponent<Rigidbody2D>();
        currentArrow = Instantiate(arrowPrefab);
        arrowAnimator = currentArrow.GetComponentInChildren<Animator>();
        currentArrow.SetActive(false);
        amiView= GetComponent<AmiView>();

    }
    private void Start()
    {
        //StartCoroutine(IntroCinematic());
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
                movingCoroutine=StartCoroutine(MoveAmi(topLeft.position, topRight.position, duration));
                break;
            case AmiPath.Bottom_RtoL:
                movingCoroutine = StartCoroutine(MoveAmi(bottomRight.position, bottomLeft.position, duration));
                break;
            case AmiPath.Left_BtoT:
                movingCoroutine = StartCoroutine(MoveAmi(leftBottom.position, leftTop.position, duration));
                break;
            case AmiPath.Right_TtoB:
                movingCoroutine = StartCoroutine(MoveAmi(rightTop.position, rightBottom.position, duration));
                break;

        }
    }
    public void ReturnToStart(AmiPath path, float duration)
    {
        switch (path)
        {
            case AmiPath.Top_LtoR:
                movingCoroutine = StartCoroutine(MoveAmi(topRight.position, topLeft.position, duration));
                break;
            case AmiPath.Bottom_RtoL:
                movingCoroutine = StartCoroutine(MoveAmi(bottomLeft.position, bottomRight.position, duration));
                break;
            case AmiPath.Left_BtoT:
                movingCoroutine = StartCoroutine(MoveAmi(leftTop.position, leftBottom.position, duration));
                break;
            case AmiPath.Right_TtoB:
                movingCoroutine = StartCoroutine(MoveAmi(rightBottom.position, rightTop.position, duration));
                break;
        }
    }
    private IEnumerator MoveAmi(Vector3 startPos, Vector3 endPos, float duration)
    {
        amiView.SetAmiMoving();
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
            currentMovementCoroutine=StartCoroutine(HitSequence());
            bulletMovement.DestroyBulletsOnHazards();
        }
    }
    public void PhaseChange()
    {
        amiView.UpdateAmiPhase();
    }
    public IEnumerator HitSequence()
    {
        Vector2 actualVelocity= rigidBody.linearVelocity;
        amiView.SetAmiHit();
        rigidBody.linearVelocity= Vector2.zero;
        if(AmiFightManager.Instance.currentPhase==1)
        {
            yield return new WaitForSeconds(amiHitStunDurationP1);
        }
        else
        {
            yield return new WaitForSeconds(amiHitStunDurationP2);
        }
        rigidBody.linearVelocity= actualVelocity;
        amiView.SetAmiMoving();
        yield break;
    }
    public IEnumerator IntroCinematic()
    {
        Debug.Log("Starting Intro Cinematic");
        movingCoroutine = StartCoroutine(MoveAmi(originalPoint.position, cutscenePoint.position, amiEnterTimeP1));
        yield return new WaitForSeconds(amiEnterTimeP1);
        Debug.Log("Ami Entered");
        amiView.SetAmiAttacking();
        Debug.Log("Ami Attacking");
        yield return new WaitForSeconds(amiStayTimeP1);
        Debug.Log("Starting Boss Fight");
        movingCoroutine = StartCoroutine(MoveAmi(cutscenePoint.position, originalPoint.position, amiExitTimeP1));
        yield return new WaitForSeconds(amiExitTimeP1);
        StartCoroutine(AmiFightManager.Instance.StartBossFight());
        yield break;
    }
    public IEnumerator ExitCinematic()
    {
        Debug.Log("Starting Exit Cinematic");
        movingCoroutine = StartCoroutine(MoveAmi(originalPoint.position, cutscenePoint.position, amiEnterTimeP2));
        yield return new WaitForSeconds(amiEnterTimeP2);
        Debug.Log("Ami Entered");
        amiView.SetAmiDeath();
        Debug.Log("Ami dying");
        yield return new WaitForSeconds(amiStayTimeP2);
        Debug.Log("Ami going out of scene");
        movingCoroutine = StartCoroutine(MoveAmi(cutscenePoint.position, endPoint.position, amiExitTimeP2));
        yield return new WaitForSeconds(amiExitTimeP2);
        yield break;

    }
    private void Update()
    {
        if(startedFight)
        {
            StartCoroutine(IntroCinematic());
            startedFight = false;
        }
    }
}
