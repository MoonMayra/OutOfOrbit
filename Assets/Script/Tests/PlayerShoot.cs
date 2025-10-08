using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private InputActionReference activateInput;

    [Header("Scripts")]
    [SerializeField] private PlayerGravityFields gravityField;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TreyectoryPreview trayectory;

    [Header("BulletMovement")]
    [SerializeField] private float bulletVel = 5;
    [SerializeField] private float accelTime = 5;
    [SerializeField] private int bulletCount = 0;
    [SerializeField] public int maxBullet = 3;
    [SerializeField] private float activeTime = 0;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;

    private float timer = 0.0f;
    private Vector2 mousePos;
    private bool isAiming = false;
    private Vector2 lineDir;

    private void Awake()
    {
        shootInput.action.started += HandleShootInput;
        shootInput.action.performed += HandleShootInput;
        shootInput.action.canceled += HandleShootInput;
        activateInput.action.performed += HandleActivateInput;

    }

    private void HandleShootInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isAiming = true;
        }
        else if(context.canceled)
        {
            isAiming = false;
        }
    }

    private void HandleActivateInput(InputAction.CallbackContext context)
    {



    }   
    
    private void BulletMovement()
    {


    }

    private void ActivateField()
    {


    }

    private void CalculateGravity()
    {

    }

    private void RenderLine()
    {
        if (bulletSpawn == null)
            return;
        trayectory.DrawTrayectory(new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y), lineDir);

    }

    private void Update()
    {
        mousePos=Input.mousePosition;
        lineDir = Camera.main.ScreenToWorldPoint(mousePos) - bulletSpawn.position;
    }

    private void FixedUpdate()
    {
        if (isAiming)
        {
            RenderLine();
        }
        else
        {
            trayectory.ClearLine();
        }
    }

}
