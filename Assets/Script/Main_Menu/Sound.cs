using UnityEngine;

    [SerializeField]
    public class Sound
    {
        [HideInInspector] public AudioSource source;
        public string clipName;
        public AudioClip audioClip;
        public bool isLoop;
        public bool playOnAwake;
        [Range(0, 1)]
        public float volume = 0.5f;
    }
