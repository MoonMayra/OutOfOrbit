using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenesActions : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = string.Empty;
    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private float particleTime = 1.0f;
    [SerializeField] private GameObject objects;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float time;
    [SerializeField] private string animKey = string.Empty;


    private Coroutine currenteCoroutine;

    public void ParticleAndLoad()
    {
        currenteCoroutine= StartCoroutine(ParticleAndLoadCoroutine());
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
        animator.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
    }
    public void PlayBoolAnimation()
    {
        animator.SetBool(animKey,true);
    }
    public void PlayTriggerAnimationLeak()
    {
        animator.SetTrigger(animKey);
        CameraShake.Instance.Shake(0.5f);
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

        while(timer<time)
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


}
