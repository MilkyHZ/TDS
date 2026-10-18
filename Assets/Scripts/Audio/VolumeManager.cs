using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager I;

    [SerializeField] private AudioMixer _audioMixer;

    private const string MASTERVOLUME = "MasterVolume";
    private const string MUSICVOLUME = "MusicVolume";
    private const string SFXVOLUME = "SFXVolume";

    private void Awake()
    {
        I = this;
        ApplySavedVolumes();
    }

    public void ApplySavedVolumes()
    {
        float master = PlayerPrefs.GetFloat(MASTERVOLUME, 1f);
        float music = PlayerPrefs.GetFloat(MUSICVOLUME, 1f);
        float sfx = PlayerPrefs.GetFloat(SFXVOLUME, 1f);

        SetMixerVolume(MASTERVOLUME, master);
        SetMixerVolume(MUSICVOLUME, music);
        SetMixerVolume(SFXVOLUME, sfx);
    }

    public void SetVolume(string parameter, float value)
    {
        SetMixerVolume(parameter, value);
        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();
    }

    private void SetMixerVolume(string parameter, float value)
    {
        if (_audioMixer == null) return;

        float safeValue = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(safeValue) * 20f;
        _audioMixer.SetFloat(parameter, dB);
    }
}