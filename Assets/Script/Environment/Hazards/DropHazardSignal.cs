using UnityEngine;

public class DropHazardSignal : MonoBehaviour
{
    public Transform targetPos;
    public Camera cameraMain;
    public RectTransform canvasRect;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(targetPos==null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 viewportTargetPos=cameraMain.WorldToViewportPoint(targetPos.position);

        if(viewportTargetPos.x<0f||viewportTargetPos.x>1f)
        {
            Destroy(gameObject );
            return;
        }

        if(viewportTargetPos.y > 1f)
        {
            float signalX = Mathf.Clamp01(viewportTargetPos.x);

            float canvasX = (signalX * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f);
            rectTransform.anchoredPosition = new Vector2(canvasX, 0f);
            return;
        }

            Destroy (gameObject);
            return;

    }
}
