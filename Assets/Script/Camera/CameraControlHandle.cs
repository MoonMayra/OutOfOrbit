using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControlHandle : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference changeInput;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraManualMov cameraManualMov;
    [SerializeField] private CameraController cameraController;
    
    [Header("Camera bounds")]
    [SerializeField] private Vector2 thisAreaCenter = Vector2.zero;
    [SerializeField] private Vector2 thisAreaSize = new Vector2(20f, 30f);

    [Header("Animator")]
    [SerializeField] private string animatorBoolName = "MovementActive";

    private bool isInArea = false;
    private bool cameraManualEnabled = false;

    private Transform cameraOriginalTransform;

    private Animator animator;

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = PlayerMovement.Instance;
        if (cameraManualMov == null)
            cameraManualMov = Camera.main.GetComponent<CameraManualMov>();
        if (cameraController == null)
            cameraController = CameraController.Instance;
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        changeInput.action.performed += OnChangeInput;
        changeInput.action.Enable();

    }
    private void OnDisable()
    {
        changeInput.action.performed -= OnChangeInput;
        changeInput.action.Disable();

    }
    
    private void OnChangeInput(InputAction.CallbackContext context)
    {
        if (!isInArea)
            return;

        cameraManualEnabled = !cameraManualEnabled;
        if (cameraManualEnabled)
            EnableCameraMode();
        else
            EnablePlayerMode();
       
    }
    
    private void EnableCameraMode()
    {
        if(cameraOriginalTransform == null)
        {
            GameObject camPosHolder = new GameObject("CameraOriginalPosHolder");
            cameraOriginalTransform = camPosHolder.transform;
        }

        cameraOriginalTransform.position = Camera.main.transform.position;
        animator.SetBool(animatorBoolName, true);
        cameraController.ReleaseControl();
        if (playerMovement)
            playerMovement.enabled = false;

        cameraManualMov.SetBounds(thisAreaCenter, thisAreaSize);
        if (cameraManualMov)
            cameraManualMov.enabled = true;
    }
    private void EnablePlayerMode()
    {
        animator.SetBool(animatorBoolName, false);
        cameraController.MoveToTarget(cameraOriginalTransform, 5f);
        if(playerMovement)
            playerMovement.enabled = true;
        if(cameraManualMov)
            cameraManualMov.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = false;
            if (cameraManualEnabled)
            {
                EnablePlayerMode();
                cameraManualEnabled = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
        Gizmos.DrawCube(thisAreaCenter, thisAreaSize);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(thisAreaCenter, thisAreaSize);
    }
}
