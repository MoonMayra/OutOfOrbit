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
    [SerializeField] private TrayectoryPreview trayectory;
    [SerializeField] private BulletMovement bulletMov;

    [Header("BulletMovement")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float spawnOffset = 0.248f;
    [SerializeField] private GameObject[] bulletPrefab = new GameObject[3];

    [Header("Voids")]
    [SerializeField] private Vector2 voidSpawn;
    [SerializeField] private GameObject[] voidPrefab = new GameObject[3];

    //Aiming variables
    private Vector3 mousePos;
    public bool isAiming = false;
    private Vector2 lineDir;
    

    //Shooting variables
    public int freeSlot = -1;
    public GameObject[] activeBullets = new GameObject[3];
    public bool isAbleToShoot = true;
    public bool shootButtonRealesed = true;

    //Voids variables
    public GameObject[] activeVoids = new GameObject[3];

    //List for indexing bullets
    public List<GameObject> bulletCreationOrder = new List<GameObject>();

    //Player reference
    private Rigidbody2D playerRB;
    public bool isLookingRight = true;
    private Transform spawn;

    private void Awake()
    {
        shootInput.action.started += HandleShootInput;
        shootInput.action.performed += HandleShootInput;
        shootInput.action.canceled += HandleShootInput;

        activateInput.action.performed += HandleActivateInput;

        playerRB = GetComponent<Rigidbody2D>();
        trayectory = GetComponentInChildren<TrayectoryPreview>();
        spawn = bulletSpawn;
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
        int previousBulletIndex = (freeSlot) % 3;

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
        trayectory.DrawTrayectory(new Vector2(spawn.transform.position.x, spawn.transform.position.y), lineDir);

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
        bool bulletdDestroyed = false;
        if (bulletCreationOrder.Count >= activeBullets.Length && !bulletdDestroyed)
        {
            GameObject oldest = bulletCreationOrder[0];
            bulletCreationOrder.RemoveAt(0);

            int slotIndex = System.Array.IndexOf(activeBullets, oldest);
            if (slotIndex != -1)
                RemoveBullet(slotIndex, true);
            bulletdDestroyed = true;
        }

        freeSlot = -1;
        for (int i = 0; i < activeBullets.Length; i++)
        {
            if (activeBullets[i] == null)
            {
                freeSlot = i;
                break;
            }
        }

        if (freeSlot == -1)
            freeSlot = 0;

        GameObject newBullet = Instantiate(bulletPrefab[freeSlot], spawn.position, Quaternion.identity);
        Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
        bulletRB.linearVelocity = lineDir.normalized;

        BulletMovement bulletMov = newBullet.GetComponent<BulletMovement>();
        bulletMov.direction = lineDir.normalized;
        bulletMov.playerShoot = this;
        bulletMov.index=freeSlot;


        activeBullets[freeSlot] = newBullet;
        bulletCreationOrder.Add(newBullet);

        UpdateShootAvailability();

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
            Destroy(activeVoids[validIndex]);
        }
        voidSpawn = bulletToUse.transform.position;
        GameObject newVoid = Instantiate(voidPrefab[validIndex], voidSpawn, Quaternion.identity);
        gravityVoid = newVoid.gameObject.GetComponent<GravityVoid>();
        gravityVoid.linkedBullet = bulletToUse;

        activeVoids[validIndex] = newVoid;
    }

    public void RemoveBullet(int slotIndex, bool immediate)
    {
        GameObject bullet = activeBullets[slotIndex];
        if (bullet != null)
        {
            bulletCreationOrder.Remove(bullet);
            if (immediate)
                DestroyImmediate(bullet);
            else
                Destroy(bullet);

            activeBullets[slotIndex] = null;
        }

        if (activeVoids[slotIndex] != null)
        {
            if (immediate)
                DestroyImmediate(activeVoids[slotIndex]);
            else
                Destroy(activeVoids[slotIndex]);

            activeVoids[slotIndex] = null;
        }

        UpdateShootAvailability();
    }
    private void Update()
    {
        mousePos=Input.mousePosition;
        lineDir = Camera.main.ScreenToWorldPoint(mousePos) - spawn.position;
    }

    private void FixedUpdate()
    {
        if (isAiming)
        {
            RenderLine();
        }
       
    }

}
