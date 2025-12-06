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

    private Coroutine currenteCoroutine;

    public void ParticleAndLoad()
    {
        currenteCoroutine = StartCoroutine(ParticleAndLoadCoroutine());
    }

    public void PlayParticle()
    {
        particleSys.Play();
    }

    public IEnumerator ParticleAndLoadCoroutine()
    {
        particleSys.Play();
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(particleTime);
        FadeScript.Instance.FadeOut(sceneToLoad);
    }

    public void PlayTriggerAnimationGorilla()
    {
        animator1.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
    }

    public void PlayBoolAnimation()
    {
        animator1.SetBool(animKey, true);
    }
    public void PlayTriggerAnimationLeak()
    {
        animator1.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
        PlayLeakSound(); 
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

    public void MoveAmiCut()
    {
        if (currenteCoroutine != null)
        {
            StopCoroutine(currenteCoroutine);
        }
        currenteCoroutine = StartCoroutine(MoveAmiCoroutine());
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

    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void AnimateLights()
    {
        animator1.SetTrigger(animKey);
        animator2.SetTrigger(animKey);
        animator3.SetTrigger(animKey);
    }
}
