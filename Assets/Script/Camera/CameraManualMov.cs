using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManualMov : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference MovementInput;
    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [Header("Bounds")]
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 30f);

    private Camera cameraComp;
    private Vector2 input;

    private void Awake()
    {
        cameraComp = Camera.main;
    }
    private void OnEnable()
    {
        MovementInput.action.started += HandleMovement;
        MovementInput.action.performed += HandleMovement;
        MovementInput.action.canceled += HandleMovement;
        MovementInput.action.Enable();
    }
    private void OnDisable()
    {
        MovementInput.action.started -= HandleMovement;
        MovementInput.action.performed -= HandleMovement;
        MovementInput.action.canceled -= HandleMovement;
        MovementInput.action.Disable();
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
       input = context.ReadValue<Vector2>();
    }
    public void SetBounds(Vector2 center, Vector2 size)
    {   
        areaCenter = center;
        areaSize = size;
    }
    private void Update()
    {
        if (cameraComp == null)
            return;

        Vector3 movementStep= new Vector3(input.x,input.y,0f) * MovementSpeed * Time.deltaTime;
        Vector3 targetPosition=cameraComp.transform.position + movementStep;

        float halfHeight = cameraComp.orthographicSize;
        float halfWidth = halfHeight * cameraComp.aspect;
        Vector2 minBounds = areaCenter - areaSize * 0.5f;
        Vector2 maxBounds = areaCenter + areaSize * 0.5f;

        bool lockX = (halfWidth * 2f) >= areaSize.x;
        bool lockY = (halfHeight * 2) >= areaSize.y;

        if(lockX)
        {
            targetPosition.x = areaCenter.x;
        }
        else
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        }

        if (lockY)
        {
            targetPosition.y = areaCenter.y;
        }
        else
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        }

        cameraComp.transform.position = targetPosition;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }

}
