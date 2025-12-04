using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenesActions : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = string.Empty;
    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private float particleTime = 1.0f;
    [SerializeField] private GameObject npc;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
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
        SceneManager.LoadScene(sceneToLoad);
    }





}
