using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource _sfxSource;

    [SerializeField]
    private AudioSource _musicSource;

    [Header("Background Music Clips")]
    [SerializeField]
    private AudioClip _menuMusic;

    [SerializeField]
    private AudioClip _gameplayMusic;

    [Header("Turret Fire Clips")]
    [SerializeField]
    private AudioClip[] _ballistaFireClips;

    [SerializeField]
    private AudioClip[] _cannonFireClips;

    [SerializeField]
    private AudioClip[] _slowTurretFireClips;

    [Header("Impact & Hit Clips")]
    [SerializeField]
    private AudioClip[] _enemyHitClips;

    [SerializeField]
    private AudioClip[] _enemyDeathClips;

    [SerializeField]
    private AudioClip[] _playerDamageClips;

    [Header("UI & Economy Clips")]
    [SerializeField]
    private AudioClip[] _coinCollectClips;

    [SerializeField]
    private AudioClip _buttonClickClip;

    [SerializeField]
    private AudioClip _buildTurretClip;

    [SerializeField]
    private AudioClip _upgradeTurretClip;

    [SerializeField]
    private AudioClip _sellTurretClip;

    [Header("Stingers")]
    [SerializeField]
    private AudioClip _winStinger;

    [SerializeField]
    private AudioClip _gameOverStinger;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Random SFX Player

    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f, float pitchMin = 0.95f, float pitchMax = 1.05f)
    {
        if (_sfxSource == null || clip == null) return;

        _sfxSource.pitch = Random.Range(pitchMin, pitchMax);
        _sfxSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayRandomSFX(AudioClip[] clips, float volumeScale = 1.0f)
    {
        if (clips == null || clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        PlaySFX(clips[randomIndex], volumeScale);
    }

    #endregion

    #region Action Audio Triggers

    public void PlayBallistaFire() => PlayRandomSFX(_ballistaFireClips);
    public void PlayCannonFire() => PlayRandomSFX(_cannonFireClips);
    public void PlaySlowTurretFire() => PlayRandomSFX(_slowTurretFireClips);

    public void PlayEnemyHit() => PlayRandomSFX(_enemyHitClips, 0.7f);
    public void PlayEnemyDeath() => PlayRandomSFX(_enemyDeathClips);
    public void PlayPlayerDamage() => PlayRandomSFX(_playerDamageClips);

    public void PlayCoinCollect() => PlayRandomSFX(_coinCollectClips, 0.8f);
    public void PlayButtonClick() => PlaySFX(_buttonClickClip);
    public void PlayBuildTurret() => PlaySFX(_buildTurretClip);
    public void PlayUpgradeTurret() => PlaySFX(_upgradeTurretClip);
    public void PlaySellTurret() => PlaySFX(_sellTurretClip);

    public void PlayWinStinger() => PlaySFX(_winStinger);
    public void PlayGameOverStinger() => PlaySFX(_gameOverStinger);

    #endregion

    #region Music Player

    public void PlayMenuMusic() => SwitchMusic(_menuMusic);
    public void PlayGameplayMusic() => SwitchMusic(_gameplayMusic);

    private void SwitchMusic(AudioClip newClip)
    {
        if (_musicSource == null || newClip == null || _musicSource.clip == newClip) return;

        _musicSource.clip = newClip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    #endregion
}