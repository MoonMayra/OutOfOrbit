using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SOUNDS = "SoundsVolume";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(SetSoundsVolume);
    }

    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetSoundsVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(value) * 20);
    }
}
