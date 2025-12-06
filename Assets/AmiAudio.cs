using UnityEngine;

public class AmiAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Idle Sounds")]
    public AudioClip[] idleClips;

    [Header("Move Sounds")]
    public AudioClip[] moveClips;

    [Header("Attack Sounds")]
    public AudioClip[] attackClips;

    [Header("Hit Sounds")]
    public AudioClip[] hitClips;

    [Header("Death Sounds")]
    public AudioClip[] deathClips;

    [Header("Fall Sounds (Solo P2)")]
    public AudioClip[] fallClips;

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
    public void PlayIdle() => PlayRandom(idleClips);
    public void PlayMove() => PlayRandom(moveClips);
    public void PlayAttack() => PlayRandom(attackClips);
    public void PlayHit() => PlayRandom(hitClips);
    public void PlayDeath() => PlayRandom(deathClips);
    public void PlayFall() => PlayRandom(fallClips);
}

