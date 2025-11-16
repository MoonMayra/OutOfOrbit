using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource DeathSource;
    [SerializeField] List<AudioClip> deathClips = new List<AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeathSound()
    {
        AudioClip clip = deathClips[Random.Range(0, deathClips.Count)];
        DeathSource.PlayOneShot(clip);
    }
}
