using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shootInput;

    [Header("Scripts")]
    [SerializeField] private PlayerGravityFields gravityField;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TrayectoryPreview trayectory;
    [SerializeField] private BulletMovement bulletMov;

    [Header("BulletMovement")]
    [SerializeField] private int bulletCount = 0;
    [SerializeField] public int maxBullet = 3;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject[] bulletPrefab = new GameObject[3];

    private Vector2 mousePos;
    private bool isAiming = false;
    private Vector2 lineDir;
    private int nextBulletIndex = 0;
    private GameObject[] activeBullets = new GameObject[3];

    private void Awake()
    {
        shootInput.action.started += HandleShootInput;
        shootInput.action.performed += HandleShootInput;
        shootInput.action.canceled += HandleShootInput;

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
            ShootBullet();
        }
    }
  
    private void RenderLine()
    {
        if (bulletSpawn == null)
            return;
        trayectory.DrawTrayectory(new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y), lineDir);

    }

    private void ShootBullet()
    {
        trayectory.ClearLine();
        Debug.Log("Voy a crear una bala");
        CreateBullet();

    }

    public void CreateBullet()
    {
        if (activeBullets[nextBulletIndex] != null) // if there's an active void in this slot, destroy it :p
        {
            Destroy(activeBullets[nextBulletIndex]);

        }

        GameObject newBullet = null;
        Rigidbody2D nextBulletRb=null;
        bulletMov=null;
        switch (nextBulletIndex)
        {
            case 0:
                newBullet = Instantiate(bulletPrefab[0], bulletSpawn.position, Quaternion.identity);
                bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
                nextBulletRb = newBullet.GetComponent<Rigidbody2D>();
                nextBulletRb.linearVelocity = lineDir.normalized;
                bulletMov.direction = lineDir.normalized;
                break;
            case 1:
                newBullet = Instantiate(bulletPrefab[1], bulletSpawn.position, Quaternion.identity);
                bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
                nextBulletRb = newBullet.GetComponent<Rigidbody2D>();
                nextBulletRb.linearVelocity = lineDir.normalized;
                bulletMov.direction = lineDir.normalized;
                break;
            case 2:
                newBullet = Instantiate(bulletPrefab[2], bulletSpawn.position, Quaternion.identity);
                bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
                nextBulletRb = newBullet.GetComponent<Rigidbody2D>();
                nextBulletRb.linearVelocity = lineDir.normalized;
                bulletMov.direction = lineDir.normalized;
                break;
        }

        activeBullets[nextBulletIndex] = newBullet;

        nextBulletIndex = (nextBulletIndex + 1) % 3; // infinite ! :D
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
    }

}
