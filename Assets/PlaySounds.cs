using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    [SerializeField] private AudioSource amiAudioSource;
    [SerializeField] private float minPitch = 1.0f;
    [SerializeField] private float maxPitch = 1.0f;

    public void Play(AudioClip soundToPlay)
    {
        if (soundToPlay != null)
        {
            PlayOneShot(soundToPlay);
        }
    }
    private void PlayOneShot(AudioClip clip)
    {
        if (amiAudioSource == null)
        {
            Debug.LogWarning("[CharacterSoundController] Missing oneShotSource ");
            return;
        }
        if (clip == null) return;

        amiAudioSource.pitch = Random.Range(minPitch, maxPitch);
        amiAudioSource.PlayOneShot(clip);
    }
}
