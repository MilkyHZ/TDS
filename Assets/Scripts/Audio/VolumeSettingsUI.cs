using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private const string MASTERVOLUME = "MasterVolume";
    private const string MUSICVOLUME = "MusicVolume";
    private const string SFXVOLUME = "SFXVolume";

    private void OnEnable()
    {
        LoadSavedValues();

        if (_masterSlider != null) _masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (_musicSlider != null) _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (_sfxSlider != null) _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnDisable()
    {
        if (_masterSlider != null) _masterSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        if (_musicSlider != null) _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (_sfxSlider != null) _sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }

    private void LoadSavedValues()
    {
        if (_masterSlider != null) _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(MASTERVOLUME, 1f));
        if (_musicSlider != null) _musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(MUSICVOLUME, 1f));
        if (_sfxSlider != null) _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(SFXVOLUME, 1f));
    }

    public void OnMasterVolumeChanged(float value)
    {
        if (VolumeManager.I != null)
        {
            VolumeManager.I.SetVolume(MASTERVOLUME, value);
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (VolumeManager.I != null)
        {
            VolumeManager.I.SetVolume(MUSICVOLUME, value);
        }
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (VolumeManager.I != null)
        {
            VolumeManager.I.SetVolume(SFXVOLUME, value);
        }
    }
}