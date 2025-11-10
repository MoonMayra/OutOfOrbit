using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public bool isAiming = false;
    private Vector2 lineDir;

    //Shooting variables
    public int nextBulletIndex = 0;
    public GameObject[] activeBullets = new GameObject[3];
    public bool isAbleToShoot = true;
    public bool shootButtonRealesed = true;

    //Voids variables
    public GameObject[] activeVoids = new GameObject[3];

    //List for indexing bullets
    private List<GameObject> bulletCreationOrder = new List<GameObject>();

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
    private void OnEnable()
    {
        shootInput.action.started += HandleShootInput;
        shootInput.action.performed += HandleShootInput;
        shootInput.action.canceled += HandleShootInput;

        activateInput.action.performed += HandleActivateInput;

        playerRB = GetComponent<Rigidbody2D>();
        trayectory = GetComponentInChildren<TrayectoryPreview>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        shootInput.action.started -= HandleShootInput;
        shootInput.action.performed -= HandleShootInput;
        shootInput.action.canceled -= HandleShootInput;

        activateInput.action.performed -= HandleActivateInput;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        trayectory = null;
        playerRB = null;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerRB == null)
        {
            playerRB = GetComponent<Rigidbody2D>();

        }
        if (trayectory == null)
        {
            trayectory = GetComponentInChildren<TrayectoryPreview>();
        }
    }
    private void HandleActivateInput(InputAction.CallbackContext context)
    {
        if(!isAbleToShoot || IsAnyBulletMoving())
            return;

        CreateVoid();
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

    public void UpdateShootAvailability()
    {
        isAbleToShoot = !IsAnyBulletMoving();
        shootButtonRealesed=!IsAnyBulletMoving();

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
        if(activeBullets[nextBulletIndex]!= null)
        {
            RemoveBullet(nextBulletIndex);
        }

        GameObject newBullet = Instantiate(bulletPrefab[nextBulletIndex], bulletSpawn.position, Quaternion.identity);
        Rigidbody2D bulletRigidBody = newBullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.linearVelocity = lineDir.normalized;
        var bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
        bulletMov.direction = lineDir.normalized;
        bulletMov.playerShoot = this;

        activeBullets[nextBulletIndex] = newBullet;
        bulletCreationOrder.Add(newBullet);

        nextBulletIndex = (nextBulletIndex + 1) % activeBullets.Length; // infinite ! :D
        UpdateShootAvailability();

        /*if (activeBullets[nextBulletIndex]!= null)
        {
            RemoveBullet(nextBulletIndex);
        }

        GameObject newBullet= Instantiate(bulletPrefab[nextBulletIndex], bulletSpawn.position, Quaternion.identity);
        Rigidbody2D bulletRigidBody = newBullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.linearVelocity = lineDir.normalized;
        var bulletMov = newBullet.gameObject.GetComponent<BulletMovement>();
        bulletMov.direction = lineDir.normalized;
        bulletMov.playerShoot = this;

        activeBullets[nextBulletIndex] = newBullet;

        bulletCreationOrder.Add(nextBulletIndex);

        nextBulletIndex = (nextBulletIndex + 1) % activeBullets.Length; // infinite ! :D
        UpdateShootAvailability();*/


        /*if (activeBullets[nextBulletIndex] != null) // if there's an active void in this slot, destroy it :p
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
        UpdateShootAvailability();*/

    }

    public void CreateVoid()
    {
        GameObject bulletToUse = null;

        foreach (var bullet in bulletCreationOrder)
        {
            if (bullet != null)
            {
                int slotIndex = System.Array.IndexOf(activeBullets, bullet);
                if(slotIndex!=-1 &&activeVoids[slotIndex]==null)
                {
                    bulletToUse = bullet;
                    break;
                }
            }
        }          
        if(bulletToUse==null)
        {
            Debug.Log("No bullets to link void to");
            return;
        }

        int validIndex = System.Array.IndexOf(activeBullets, bulletToUse);
        if(activeVoids[validIndex]!=null)
        {
            Debug.Log("Destroying previous void");
            Destroy(activeVoids[validIndex]);
        }
        voidSpawn = bulletToUse.transform.position;
        GameObject newVoid = Instantiate(voidPrefab[validIndex], voidSpawn, Quaternion.identity);
        gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
        gravityVoid.linkedBullet = bulletToUse;

        activeVoids[validIndex] = newVoid;

        /*int validIndex = -1;

        foreach(var slotIndex in bulletCreationOrder)
        {
            if(activeBullets[slotIndex]!=null && activeVoids[slotIndex]==null)
            {
                validIndex = slotIndex;
                break;
            }
        }

        if(validIndex==-1)
        {
            Debug.Log("No bullets to link void to");
            return;
        }

        if(activeVoids[validIndex]!=null)
        {
            Debug.Log("Destroying previous void");
            Destroy(activeVoids[validIndex]);
        }

        voidSpawn = activeBullets[validIndex].transform.position; 
        GameObject newVoid = Instantiate(voidPrefab[validIndex], voidSpawn, Quaternion.identity);
        gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
        gravityVoid.linkedBullet = activeBullets[validIndex];

        activeVoids[validIndex] = newVoid;*/
    }

    public void RemoveBullet(int slotIndex)
    {
        GameObject bullet = activeBullets[slotIndex];
        if (bullet != null)
        {
            bulletCreationOrder.Remove(bullet);
            Destroy(bullet);
            activeBullets[slotIndex] = null;
        }
        if(activeVoids[slotIndex]!=null)
        {
            Debug.Log("Destroying linked void");
            Destroy(activeVoids[slotIndex]);
            activeVoids[slotIndex] = null;
        }
        UpdateShootAvailability();

        /*if (activeBullets[slotIndex]!=null)
        {
            Destroy(activeBullets[slotIndex]);
        }
        if(activeVoids[slotIndex]!=null)
        {
            Destroy(activeVoids[slotIndex]);
            activeVoids[slotIndex] = null;
        }
        activeBullets[slotIndex] = null;
        bulletCreationOrder.Remove(slotIndex);
        UpdateShootAvailability();*/
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
