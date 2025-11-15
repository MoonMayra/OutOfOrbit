using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            if (s.playOnAwake)
                s.source.Play();
        }
    }
    // public void Play(string clipname)
     //{
     //   Sound s = Array.Find(sounds, )
    // }
}
