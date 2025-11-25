using UnityEngine;

public class SignalManager : MonoBehaviour
{
    public static SignalManager Instance { get; private set; }

    [SerializeField] private RectTransform signalPrefab;
    [SerializeField] private Canvas canvas;

    private RectTransform canvasRect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasRect = canvas.GetComponent<RectTransform>();

    }
    public void CreateSignal(Transform hazard)
    {
        Vector3 viewportPos=Camera.main.WorldToViewportPoint(hazard.position);

        if (viewportPos.x < 0f || viewportPos.x > 1f)
            return;
        if(viewportPos.y<=1f)
            return;

        RectTransform signal=Instantiate(signalPrefab, canvasRect.transform);

        DropHazardSignal signalScript=signal.GetComponent<DropHazardSignal>();
        signalScript.targetPos = hazard;
        signalScript.cameraMain = Camera.main;
        signalScript.canvasRect = canvasRect;

    }
}
