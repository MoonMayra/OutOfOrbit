using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private InputActionReference activateInput;

    [Header("Scripts")]
    [SerializeField] private GravityVoid gravityVoid;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TrayectoryPreview trayectory;
    [SerializeField] private BulletMovement bulletMov;

    [Header("BulletMovement")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject[] bulletPrefab = new GameObject[3];

    [Header("Voids")]
    [SerializeField] private Vector2 voidSpawn;
    [SerializeField] private GameObject[] voidPrefab = new GameObject[3];

    //Aiming variables
    private Vector2 mousePos;
    private bool isAiming = false;
    private Vector2 lineDir;
    //Shooting variables
    private int nextBulletIndex = 0;
    public GameObject[] activeBullets = new GameObject[3];
    private bool isAbleToShoot = true;
    //Voids variables
    private int nextVoidIndex = 0;
    public GameObject[] activeVoids = new GameObject[3];

    //Player reference
    private Rigidbody2D playerRB;

    private void Awake()
    {
        shootInput.action.started += HandleShootInput;
        shootInput.action.performed += HandleShootInput;
        shootInput.action.canceled += HandleShootInput;

        activateInput.action.performed += HandleActivateInput;

        playerRB = GetComponent<Rigidbody2D>();
    }

    private void HandleActivateInput(InputAction.CallbackContext context)
    {
        if( activeBullets[0] == null && activeBullets[1] == null && activeBullets[2] == null)
        {
            return;
        }
        if (isAbleToShoot)
        {
            CreateVoid();
        }
    }
    private void HandleShootInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isAbleToShoot)
            {
                isAiming = true;
            }
            else
            {
                StopBullet();
            }
        }
        else if (context.canceled)
        {
            if (isAbleToShoot)
            {
                isAiming = false;
                ShootBullet();
                ToggleShoot();
            }
            else
            {
                ToggleShoot();
                Debug.Log(isAbleToShoot);
            }
        }

    }
  
    private void StopBullet()
    {
        int previousBulletIndex = (nextBulletIndex - 1+activeBullets.Length) % 3; 

        if (activeBullets[previousBulletIndex] != null) 
        {

            Rigidbody2D actualBulletRB = activeBullets[previousBulletIndex].gameObject.GetComponent<Rigidbody2D>();
            actualBulletRB.linearVelocity = Vector2.zero;
            BulletMovement actualBulletMov = activeBullets[previousBulletIndex].gameObject.GetComponent<BulletMovement>();
            actualBulletMov.enabled = false;
            actualBulletRB.bodyType = RigidbodyType2D.Static;
        }
    }
    private void ToggleShoot()
    {
        isAbleToShoot=!isAbleToShoot;
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
        CreateBullet();

    }

    public void CreateBullet()
    {
        if (activeBullets[nextBulletIndex] != null) // if there's an active void in this slot, destroy it :p
        {
            Destroy(activeBullets[nextBulletIndex]);
            Destroy(activeVoids[nextBulletIndex]);

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

    private void CreateVoid()
    {
        Debug.Log("Creating void");
        int previousVoidIndex = (nextVoidIndex - 1 + activeVoids.Length) % 3; 
        if (activeVoids[nextVoidIndex] != null) 
        {
            Debug.Log("Destroying previous void");
            Destroy(activeVoids[nextVoidIndex]);
        }
        GameObject newVoid = null;
        switch (nextVoidIndex)
        {
            case 0:
                Debug.Log("Creating void 0");
                voidSpawn = new Vector2(activeBullets[0].transform.position.x, activeBullets[0].transform.position.y);
                newVoid = Instantiate(voidPrefab[0], voidSpawn, Quaternion.identity);
                gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
                gravityVoid.linkedBullet = activeBullets[0];
                break;
            case 1:
                Debug.Log("Creating void 1");
                voidSpawn = new Vector2(activeBullets[1].transform.position.x, activeBullets[1].transform.position.y);
                newVoid = Instantiate(voidPrefab[1], voidSpawn, Quaternion.identity);
                gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
                gravityVoid.linkedBullet = activeBullets[1];
                break;
            case 2:
                Debug.Log("Creating void 2");
                voidSpawn = new Vector2(activeBullets[2].transform.position.x, activeBullets[2].transform.position.y);
                newVoid = Instantiate(voidPrefab[2], voidSpawn, Quaternion.identity);
                gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
                gravityVoid.linkedBullet = activeBullets[2];
                break;
        }
        activeVoids[nextVoidIndex] = newVoid;
        nextVoidIndex = (nextVoidIndex + 1) % 3; // infinite ! :D
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
