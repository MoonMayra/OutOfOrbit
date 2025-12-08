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

    [Header("Cutscene Sounds")]
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip tvSound;
    [SerializeField] private AudioClip timeMachineSound;
    [SerializeField] private AudioClip foliageSound;
    [SerializeField] private AudioClip hitLeavesSound;
    [SerializeField] private AudioClip gorillaSound;
    [SerializeField] private AudioClip rumbleSound;
    [SerializeField] private AudioClip amiStepsSound;
    [SerializeField] private AudioClip alarmSound;

    [Header("Colliders")]
    [SerializeField] private GameObject triggerToDisable;
    [SerializeField] private GameObject triggerToEnable;

    private Coroutine currenteCoroutine;

    // ------------------------------
    // CUTSCENE 1
    // ------------------------------

    public void TVAction()
    {
        if (sfxSource && tvSound)
            sfxSource.PlayOneShot(tvSound);
    }

    public void StopTVSound()
    {
        if (sfxSource)
            sfxSource.Stop();
    }

    public void TimeMachineAction()
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

        if (sfxSource && timeMachineSound)
            sfxSource.PlayOneShot(timeMachineSound);

        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(particleTime);

        FadeScript.Instance.FadeOut(sceneToLoad);
    }

    // ------------------------------
    // CUTSCENE 3
    // ------------------------------

    public void PlayFoliageSound()
    {
        if (sfxSource && foliageSound)
            sfxSource.PlayOneShot(foliageSound);
    }

    public void PlayTriggerAnimationGorilla()
    {
        animator1.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);

        if (sfxSource && hitLeavesSound)
            sfxSource.PlayOneShot(hitLeavesSound);

        if (sfxSource && gorillaSound)
            sfxSource.PlayOneShot(gorillaSound);
    }

    // ------------------------------
    // CUTSCENE 5
    // ------------------------------

    public void RumbleAction()
    {
        CameraShake.Instance.Shake(0.5f);

        if (sfxSource && rumbleSound)
            sfxSource.PlayOneShot(rumbleSound);
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

    // ------------------------------
    // WATER UP
    // ------------------------------

    public void WaterUp()
    {
        if (currenteCoroutine != null)
            StopCoroutine(currenteCoroutine);

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
            objects.transform.position = Vector3.Lerp(start, end, timer / time);
            yield return null;
        }

        objects.transform.position = end;
    }

    // ------------------------------
    // CUTSCENE 7 — Ami Moving
    // ------------------------------

    public void MoveAmiCut()
    {
        if (currenteCoroutine != null)
            StopCoroutine(currenteCoroutine);

        if (sfxSource && amiStepsSound)
            sfxSource.PlayOneShot(amiStepsSound);

        currenteCoroutine = StartCoroutine(MoveAmiCoroutine());
    }

    private IEnumerator MoveAmiCoroutine()
    {
        if (amiRB != null)
            amiRB.linearVelocity = Vector2.zero;

        AmiView.Instance.SetAmiMoving();

        Vector2 target = endPoint.position;
        float direction = Mathf.Sign(target.x - amiRB.position.x);

        if (sceneEnd)
            blockColl.enabled = false;

        while (Mathf.Abs(target.x - amiRB.position.x) > 0.5f)
        {
            amiRB.linearVelocity = new Vector2(direction * amiVel, amiRB.linearVelocity.y);
            yield return null;
        }

        amiRB.linearVelocity = Vector2.zero;
        amiRB.position = new Vector2(target.x, amiRB.position.y);

        AmiView.Instance.SetAmiIdle();

        if (nxtColl != null)
            nxtColl.enabled = true;
    }

    // ------------------------------
    // CUTSCENE 8 — Alarms
    // ------------------------------

    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void AnimateLights()
    {
        animator1.SetTrigger(animKey);
        animator2.SetTrigger(animKey);
        animator3.SetTrigger(animKey);

        if (sfxSource && alarmSound)
            sfxSource.PlayOneShot(alarmSound);
    }
}

