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
    public bool isAbleToShoot = true;
    public bool shootButtonRealesed = true;
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
        trayectory = GetComponentInChildren<TrayectoryPreview>();
    }

    private void HandleActivateInput(InputAction.CallbackContext context)
    {
        if(IsAnyBulletMoving())
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
            if (isAbleToShoot && shootButtonRealesed)
            {
                isAiming = true;
            }
            else if (!isAbleToShoot)
            {
                StopBullet();
            }
        }
        else if (context.canceled)
        {
            shootButtonRealesed = true;
            if (isAbleToShoot && isAiming)
            {
                isAiming = false;
                ShootBullet();
                UpdateShootAvailability();
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

            UpdateShootAvailability();
        }
    }

    private void UpdateShootAvailability()
    {
        isAbleToShoot = !IsAnyBulletMoving();

    }

    private bool IsAnyBulletMoving()
    {
        foreach(var bullet in activeBullets)
        {
            if(bullet!=null)
            {
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                if(bulletRB.linearVelocity.sqrMagnitude>0.1f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void RenderLine()
    {
        if (bulletSpawn == null)
            return;
        trayectory.DrawTrayectory(new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y), lineDir);

    }

    private void ShootBullet()
    {
        shootButtonRealesed = false;
        trayectory.ClearLine();
        CreateBullet();

    }

    public void OnBulletStopped(BulletMovement stoppedBullet)
    {
        UpdateShootAvailability();
    }
    public void CreateBullet()
    {
        if (activeBullets[nextBulletIndex] != null) // if there's an active void in this slot, destroy it :p
        {
            Destroy(activeBullets[nextBulletIndex]);
            Destroy(activeVoids[nextBulletIndex]);

        }

        GameObject newBullet= Instantiate(bulletPrefab[nextBulletIndex], bulletSpawn.position, Quaternion.identity);
        bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
        Rigidbody2D nextBulletRb = newBullet.GetComponent<Rigidbody2D>();
        nextBulletRb.linearVelocity = lineDir.normalized;
        bulletMov.direction = lineDir.normalized;
        bulletMov.playerShoot = this;

        activeBullets[nextBulletIndex] = newBullet;
        nextBulletIndex = (nextBulletIndex + 1) % 3; // infinite ! :D
        UpdateShootAvailability();

    }

    private void CreateVoid()
    {
        if(activeBullets[(nextVoidIndex)] == null)
        {
            Debug.Log("No bullet to link void to");
            return;
        }
        if (activeVoids[nextVoidIndex] != null)
        {
            Debug.Log("Destroying previous void");
            Destroy(activeVoids[nextVoidIndex]);
        }
        voidSpawn = activeBullets[nextVoidIndex].transform.position; 
        GameObject newVoid = Instantiate(voidPrefab[nextVoidIndex], voidSpawn, Quaternion.identity);
        gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
        gravityVoid.linkedBullet = activeBullets[nextVoidIndex];

        activeVoids[nextVoidIndex] = newVoid;
        nextVoidIndex = (nextVoidIndex + 1) % 3; // infinite ! :D

        /*
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
        nextVoidIndex = (nextVoidIndex + 1) % 3; // infinite ! :D*/
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
