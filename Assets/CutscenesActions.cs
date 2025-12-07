using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenesActions : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = string.Empty;
    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private float particleTime = 1.0f;
    [SerializeField] private GameObject objects;
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    [SerializeField] private Animator animator3;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float time;
    [SerializeField] private string animKey = string.Empty;

    [Header("Ami Cutscene")]
    [SerializeField] private Rigidbody2D amiRB;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float amiVel;
    [SerializeField] private Collider2D nxtColl;
    [SerializeField] private bool sceneEnd = false;
    [SerializeField] private Collider2D blockColl;

    [Header("Leak Sound")]
    [SerializeField] private AudioSource leakAudioSource;
    [SerializeField] private AudioClip leakClip;

    [Header("Colliders")]
    [SerializeField] private GameObject triggerToDisable;
    [SerializeField] private GameObject triggerToEnable;

    private Coroutine currenteCoroutine;

    //Cutscene 1
    /*Sonidos:
    1-Tv
    2-Maquina del tiempo
     */
    public void TVAction()
    {
        Debug.Log("Playing TV sound");
        //Mica aca agregas el sonido de la tv de la primer cutscene (Sonido 1)
    }
    public void StopTVSound()
    {
        Debug.Log("Stoping TV sound");
        //Si se puede aca para el sonido, si no se puede avisame por favor
    }
    public void TimeMachineAction()
    {
        currenteCoroutine = StartCoroutine(ParticleAndLoadCoroutine());
    }

    public void PlayParticle()
    {
        particleSys.Play(); //Este ignoralo
    }

    public IEnumerator ParticleAndLoadCoroutine()
    {
        particleSys.Play();
        Debug.Log("Playing Time machine sound");
        //Mica aca agregas el sonido de la maquina del tiempo (Sonido 2)
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(particleTime);
        FadeScript.Instance.FadeOut(sceneToLoad);
    }
    //Cutscene 3
    /*Sonidos:
    1-Sonido de ruffle en las hojas
    2-Sonido de golpe sobre hojas
    3-Sonido de gorila
     */
    public void PlayFoliageSound()
    {
        Debug.Log("Foliage sound");
        //Aca agregas el sonido del ruffle de las hojas (Sonido 1)
    }    
    public void PlayTriggerAnimationGorilla()
    {
        animator1.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
        Debug.Log("Playing gorila sound");
        //Aca agregas el sonido del golpe y del gorila (Sonidos 2 y 3)
    }
    //Cutscene 5
    /*Sonidos:
    1-Leak (ya esta hecho bien)
    2-Sonido de retumbe tipo rumble.
     */
    public void RumbleAction()
    {
        CameraShake.Instance.Shake(0.5f);
        Debug.Log("Playing rumble sound");
        //Aca agregas el sonido del rumble
    }
    public void PlayBoolAnimation()
    {
        animator1.SetBool(animKey, true); //Este ignoralo
    }
    public void PlayTriggerAnimationLeak()
    {
        animator1.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
        PlayLeakSound();  //Este esta perfecto
    }
    private void PlayLeakSound()
    {
        if (leakAudioSource == null || leakClip == null)
            return;

        leakAudioSource.clip = leakClip;
        leakAudioSource.loop = true;
        leakAudioSource.Play();
    }
    public void WaterUp()
    {
        if (currenteCoroutine != null)
        {
            StopCoroutine(currenteCoroutine);
        }
        currenteCoroutine = StartCoroutine(WaterUpCoroutine());
    }
    private IEnumerator WaterUpCoroutine()
    {
        Vector3 start = startPosition.position;
        Vector3 end = endPosition.position;
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float interpolation = Mathf.Clamp01(timer / time);
            objects.transform.position = Vector3.Lerp(start, end, interpolation);
            yield return null;
        }
        objects.transform.position = end;
    }
    public void Shake()
    {
        CameraShake.Instance.Shake(0.5f);
    }

    //Cutscene 7
    /*sonidos:
    1-Pasos Ami 1
     */

    public void MoveAmiCut()
    {
        if (currenteCoroutine != null)
        {
            StopCoroutine(currenteCoroutine);
        }
        currenteCoroutine = StartCoroutine(MoveAmiCoroutine());
        Debug.Log("Playing Ami footsteps");
        //Aca podes poner los pasos o en la corrutina de abajo, es lo mismo, son los mismos sonidos en los distintos tramos
    }

    private IEnumerator MoveAmiCoroutine()
    {
        if (amiRB != null)
        {
            amiRB.linearVelocity = Vector2.zero;
        }
        AmiView.Instance.SetAmiMoving();

        Vector2 target = endPoint.position;
        float direction = Mathf.Sign(target.x - amiRB.position.x);

        if (sceneEnd)
        {
            blockColl.enabled = false;
        }

        while (Mathf.Abs(target.x - amiRB.position.x) > 0.5f)
        {
            amiRB.linearVelocity = new Vector2(direction * amiVel, amiRB.linearVelocity.y);
            yield return null;
        }
        amiRB.linearVelocity = Vector2.zero;
        amiRB.position = new Vector2(target.x, amiRB.position.y);

        AmiView.Instance.SetAmiIdle();
        if (nxtColl != null)
        {
            nxtColl.enabled = true;
        }
    }
    //Cutscene 8
    /*Sonidos:
    1-Alarmas sonando
     */
    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void AnimateLights()
    {
        animator1.SetTrigger(animKey);
        animator2.SetTrigger(animKey);
        animator3.SetTrigger(animKey);
        Debug.Log("Playing alarm sounds");
        //Aca podes poner el sonido de las alarmas (Sonido 1)
    }
}
