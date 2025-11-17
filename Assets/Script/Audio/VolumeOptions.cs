using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SOUNDS = "SoundsVolume";


    // Applied values
    private float appliedMusic;
    private float appliedSounds;

    // Default values
    [Header("Default Values (0–1)")]
    [SerializeField] float defaultMusicValue = 0.8f;
    [SerializeField] float defaultSoundsValue = 0.8f;

    private void Awake()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer not assigned");
            return;
        }
        if (musicSlider == null)
        {
            Debug.LogError("MusicSlider not assigned");
            return;
        }
        if (soundsSlider == null)
        {
            Debug.LogError("SoundSlider not assigned");
            return;
        }


    }
    private void Start()
    {
        // charge saved values or defaunt
        appliedMusic = PlayerPrefs.GetFloat(MIXER_MUSIC, defaultMusicValue);
        appliedSounds = PlayerPrefs.GetFloat(MIXER_SOUNDS, defaultSoundsValue);

        // show values in sliders
        musicSlider.value = appliedMusic;
        soundsSlider.value = appliedSounds;

        if (musicSlider != null) musicSlider.onValueChanged.AddListener(ApplyMusic);
        if (soundsSlider != null) soundsSlider.onValueChanged.AddListener(ApplySounds);

        // apply to the mixer
        ApplyMusic(appliedMusic);
        ApplySounds(appliedSounds);
    }
    void ApplyMusic(float value)
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void ApplySounds(float value)
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(value) * 20);
    }

    // buttons

    public void ApplySettings()
    {
        appliedMusic = musicSlider.value;
        appliedSounds = soundsSlider.value;

        ApplyMusic(appliedMusic);
        ApplySounds(appliedSounds);

        PlayerPrefs.SetFloat(MIXER_MUSIC, appliedMusic);
        PlayerPrefs.SetFloat(MIXER_SOUNDS, appliedSounds);
        PlayerPrefs.Save();
    }
    public void ResetSettings()
    {
        // Set sliders to default values
        musicSlider.value = defaultMusicValue;
        soundsSlider.value = defaultSoundsValue;

        // Apply immediately to mixer
        ApplyMusic(defaultMusicValue);
        ApplySounds(defaultSoundsValue);

        // Update applied values
        appliedMusic = defaultMusicValue;
        appliedSounds = defaultSoundsValue;

        // Save defaults
        PlayerPrefs.SetFloat(MIXER_MUSIC, appliedMusic);
        PlayerPrefs.SetFloat(MIXER_SOUNDS, appliedSounds);
        PlayerPrefs.Save();
    }

}
