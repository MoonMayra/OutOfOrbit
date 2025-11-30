using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class AmiFightManager : MonoBehaviour
{
    public static AmiFightManager Instance { get; private set; }
    [Header("References")]
    [SerializeField] private safeZonesAmi[] safeZones;
    [SerializeField] private WallMovement[] wallMovements;

    [Header("General settings")]
    [SerializeField] private float timeBetweenPhases = 4f;
    [SerializeField] private float amiChangeAnimationTime = 2f;
    [SerializeField] private AmiController ami;
    [SerializeField] private Transform amiStartPosition;

    [Header("Settings Phase 1")]
    [SerializeField] private float closeDurationP1 = 6f;
    [SerializeField] private float stillTimeP1 = 2f;
    [SerializeField] private float openDurationP1 = 2f;
    [SerializeField] private float timeBetweenSequencesP1 = 3f;


    [Header("Settings Phase 2")]
    [SerializeField] private float closeDurationP2 = 6f;
    [SerializeField] private float stillTimeP2 = 2f;
    [SerializeField] private float openDurationP2 = 2f;
    [SerializeField] private float timeBetweenSequencesP2 = 2f;


    [Header("Ami Phase 1")]
    [SerializeField] private float amiEntryDurationP1 = 6f;
    [SerializeField] private float signTimeP1 = 2f;
    [SerializeField] private int amiLivesP1 = 9;

    [Header("Ami Phase 2")]
    [SerializeField] private float amiEntryDurationP2 = 6f;
    [SerializeField] private float signTimeP2 = 2f;
    [SerializeField] private int amiLivesP2 = 5;


    private int currentSafeZoneID = 0;
    private Coroutine currentLoop;
    public int currentPhase = 1;
    private bool fightActive = false;

    private AmiController.AmiPath[] pathOrder=
    {
            AmiController.AmiPath.Top_LtoR,
            AmiController.AmiPath.Right_TtoB,
            AmiController.AmiPath.Bottom_RtoL,
            AmiController.AmiPath.Left_BtoT
    };
    private int pathIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        ami.transform.position = amiStartPosition.position;
        StartBossFight(); //Solo para pruebas, despues lo llamamos desde otro lado
    }
    public void StartBossFight()
    {
        fightActive = true;
        currentPhase = 1;
        currentLoop = StartCoroutine(Phase1Loop());
    }
    private void StopBossFight()
    {
        //Esta no se si la voy a usar a
        StopCoroutine(currentLoop);
    }
    private void ChangePhases()
    {
        fightActive = false;
        currentPhase = 2;
        if(currentLoop!=null)
        {
            StopCoroutine(currentLoop);
        }
        StopAllCoroutines();
        currentSafeZoneID = 0;
        pathIndex = 0;
        ami.HideSign();
        foreach (var zone in safeZones)
        {
            zone.DeactivateZone();
        }
        foreach (var wall in wallMovements)
        {
            wall.ReturnToInitialPosition(1f);
        }
        ami.transform.position = amiStartPosition.position;
        fightActive = true;
        StartCoroutine(ChangePhaseCoroutine());
    }
    private IEnumerator ChangePhaseCoroutine()
    {
        ami.PhaseChange();
        yield return new WaitForSeconds(amiChangeAnimationTime);

        yield return new WaitForSeconds(timeBetweenPhases);

        currentLoop = StartCoroutine(Phase2Loop());
    }
    private void ResetFight()
    {
        StopCoroutine(currentLoop);
        currentSafeZoneID = 0;
        pathIndex = 0;
        if(currentPhase==1)
        {
            currentLoop = StartCoroutine(Phase1Loop());
        }
        else if(currentPhase==2)
        {
            currentLoop = StartCoroutine(Phase2Loop());
        }
    }
    private void EndBossFight()
    {
        fightActive = false;
        if (currentLoop!=null)
        {
            StopCoroutine(currentLoop);
        }
        StopAllCoroutines();
        ami.HideSign();
        foreach(var zone in safeZones)
        {
            zone.DeactivateZone();
        }
        foreach(var wall in wallMovements)
        {
            wall.ReturnToInitialPosition(1f);
        }
        ami.transform.position=amiStartPosition.position;
        Debug.Log("Boss Fight Ended");
        //DEspues vemos si agregamos cinematica tipo paso de escena o que hacemos aca
    }
    public void AmiHit()
    {
        if(currentPhase == 1)
        {
            amiLivesP1--;
            Debug.Log("Ami Lives P1: " + amiLivesP1);
            if (amiLivesP1 <= 0)
            {
                Debug.Log("Changing to Phase 2");
                ChangePhases();
            }
        }
        else if (currentPhase == 2)
        {
            amiLivesP2--;
            Debug.Log("Ami Lives P2: " + amiLivesP2);
            if (amiLivesP2 <= 0)
            {
                Debug.Log("Ami Defeated");
                EndBossFight();
            }
        }
    }
    private IEnumerator Phase1Loop()
    {
        while (currentPhase==1)
        {
            yield return StartCoroutine(SpikesMovementCoroutine(currentSafeZoneID,closeDurationP1,stillTimeP1,openDurationP1));
            currentSafeZoneID = (currentSafeZoneID + 1) % safeZones.Length;
            yield return new WaitForSeconds(timeBetweenSequencesP1);
            yield return StartCoroutine(AmiSequenceCoroutine(signTimeP1,amiEntryDurationP1));
            yield return new WaitForSeconds(timeBetweenSequencesP1);
        }
    }
    private IEnumerator Phase2Loop()
    {
        while (currentPhase==2)
        {
            yield return StartCoroutine(SpikesMovementCoroutine(currentSafeZoneID, closeDurationP2, stillTimeP2, openDurationP2));
            currentSafeZoneID = (currentSafeZoneID + 1) % safeZones.Length;
            yield return new WaitForSeconds(timeBetweenSequencesP2);
            yield return StartCoroutine(AmiSequenceCoroutine(signTimeP2, amiEntryDurationP2));
            yield return new WaitForSeconds(timeBetweenSequencesP2);
        }
    }

    private IEnumerator SpikesMovementCoroutine(int safeZoneID, float closeDuration,float stillTime, float openDuration)
    {
        if(!fightActive)
        {
            yield break;
        }
        for (int i = 0; i < safeZones.Length; i++)
        {
            if (i == safeZoneID)
            {
                safeZones[i].ActivateZone();
            }
            else
            {
                safeZones[i].DeactivateZone();
            }
        }
        yield return new WaitForSeconds(0.5f);
        safeZonesAmi zone=safeZones[safeZoneID];
        Bounds bounds=zone.zoneCollider.bounds;

        foreach(var wall in wallMovements)
        {
            Vector3 target = CalculateTargetForWall(wall, bounds);
            wall.MoveTo(target, closeDuration);
        }
        yield return new WaitForSeconds(closeDuration + stillTime);
        foreach (var wall in wallMovements)
        {
            wall.ReturnToInitialPosition(openDuration);
        }
        yield return new WaitForSeconds(openDuration);
        safeZones[safeZoneID].DeactivateZone();
    }

    private Vector3 CalculateTargetForWall(WallMovement wall, Bounds bounds)
    {
        Vector3 target = wall.GetInitialPosition();

        BoxCollider2D tileCollider = wall.GetComponent<BoxCollider2D>();
        Vector3 halfSize = Vector3.Scale(tileCollider.size * 0.5f,wall.transform.lossyScale);


        switch (wall.GetWallSide())
        {
            case WallMovement.WallSide.Left:
                target.x = bounds.min.x - halfSize.x;
                break;
            case WallMovement.WallSide.Right:
                target.x = bounds.max.x + halfSize.x;
                break;
            case WallMovement.WallSide.Top:
                target.y = bounds.max.y + halfSize.y;
                break;
            case WallMovement.WallSide.Bottom:
                target.y = bounds.min.y - halfSize.y;
                break;
        }
        return target;
    }

    private IEnumerator AmiSequenceCoroutine(float signTime, float amiEntryDuration)
    {
        if (!fightActive) yield break;
        var path= pathOrder[pathIndex];

        ami.ShowSign(path);
        yield return new WaitForSeconds(signTime);
        if (!fightActive) yield break;
        ami.HideSign();

        ami.StartPath(path,amiEntryDuration);
        yield return new WaitForSeconds(amiEntryDuration);
        if (!fightActive) yield break;

        ami.ShowReturnSign(path);
        yield return new WaitForSeconds(signTime);
        if (!fightActive) yield break;
        ami.HideSign();

        ami.ReturnToStart(path, amiEntryDuration);
        yield return new WaitForSeconds(amiEntryDuration);
        if (!fightActive) yield break;

        pathIndex = (pathIndex + 1) % pathOrder.Length;
    }
}
