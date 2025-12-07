using UnityEngine;

public class AmiSoundController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Phase 1 Sounds")]
    public AudioClip idleP1Sound;
    public AudioClip moveP1Sound;
    public AudioClip attackP1Sound;
    public AudioClip hitP1Sound;
    public AudioClip deathP1Sound;  // Change phase sound

    [Header("Phase 2 Sounds")]
    public AudioClip idleP2Sound;
    public AudioClip moveP2Sound;
    public AudioClip attackP2Sound;
    public AudioClip hitP2Sound;
    public AudioClip deathP2Sound;

    // Events called fron animations

    public void PlayIdleP1() => Play(idleP1Sound);
    public void PlayMoveP1() => Play(moveP1Sound);
    public void PlayAttackP1() => Play(attackP1Sound);
    public void PlayHitP1() => Play(hitP1Sound);
    public void PlayDeathP1() => Play(deathP1Sound);
    public void PlayIdleP2() => Play(idleP2Sound);
    public void PlayMoveP2() => Play(moveP2Sound);
    public void PlayAttackP2() => Play(attackP2Sound);
    public void PlayHitP2() => Play(hitP2Sound);
    public void PlayDeathP2() => Play(deathP2Sound);
    private void Play(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
