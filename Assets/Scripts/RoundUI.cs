using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    [Header("Round Transition Splash")]
    [SerializeField] private float _transitionTime = 2f;
    [SerializeField] private RoundSplashUI _splashUI;

    [Header("Wave Info HUD")]
    [SerializeField] private TextMeshProUGUI _txtWaveNumber;
    [SerializeField] private TextMeshProUGUI _txtTotalEnemiesCount;

    [Header("Enemy Preview List Container")]
    [SerializeField] private Transform _enemyCardsContainer;
    [SerializeField] private EnemyPreviewCard _enemyCardPrefab;

    [Header("Controls")]
    [SerializeField] private Button _btnNextWave;

    private float _splashTimeRemaining = 0f;
    private bool _isSplashActive = false;
    private int _lastBanneredWave = -1;

    private void Awake()
    {
        if (_btnNextWave != null) _btnNextWave.onClick.AddListener(OnClickNextWave);
        if (_splashUI != null) _splashUI.HideBanner();
    }

    private void OnEnable()
    {
        if (RoundManager.I != null)
        {
            RoundManager.I.OnRoundDataUpdated += UpdateRoundUI;
            RoundManager.I.OnWaveStarted += TriggerSplashBanner;

            if (RoundManager.I.CurrentRoundData != null)
            {
                UpdateRoundUI(RoundManager.I.TotalRounds + 1, RoundManager.I.CurrentRoundData);
            }

            int currentWaveIndex = RoundManager.I.TotalRounds + 1;
            if (RoundManager.I.IsWaveInProgress && _lastBanneredWave != currentWaveIndex)
            {
                TriggerSplashBanner(currentWaveIndex);
            }
        }
    }

    private void OnDisable()
    {
        if (RoundManager.I != null)
        {
            RoundManager.I.OnRoundDataUpdated -= UpdateRoundUI;
            RoundManager.I.OnWaveStarted -= TriggerSplashBanner;
        }
    }

    private void Update()
    {
        if (!_isSplashActive) return;

        if (GameControler.I != null && GameControler.I.GetCurrentState() is PlayingState)
        {
            _splashTimeRemaining -= Time.unscaledDeltaTime;

            if (_splashTimeRemaining <= 0f)
            {
                _isSplashActive = false;
                if (_splashUI != null) _splashUI.HideBanner();
            }
        }
    }

    private void UpdateRoundUI(int waveNumber, RoundData roundData)
    {
        if (_txtWaveNumber != null) _txtWaveNumber.text = $"WAVE {waveNumber}";
        if (roundData == null) return;

        if (_txtTotalEnemiesCount != null)
        {
            _txtTotalEnemiesCount.text = $"Total Enemies: {roundData.GetTotalCreatureCount()}";
        }

        if (_enemyCardsContainer != null)
        {
            foreach (Transform child in _enemyCardsContainer) Destroy(child.gameObject);

            if (_enemyCardPrefab != null && roundData.Creatures != null)
            {
                foreach (var creature in roundData.Creatures)
                {
                    if (creature.Type == null) continue;
                    EnemyPreviewCard card = Instantiate(_enemyCardPrefab, _enemyCardsContainer);
                    card.Setup(creature.Type.PreviewIcon, creature.SpawnCount);
                }
            }
        }

        if (_btnNextWave != null) _btnNextWave.interactable = !RoundManager.I.IsWaveInProgress;
    }

    private void TriggerSplashBanner(int waveNumber)
    {
        _lastBanneredWave = waveNumber;
        _splashTimeRemaining = _transitionTime;
        _isSplashActive = true;

        if (_splashUI != null)
        {
            bool shouldSkipTypewriter = (waveNumber == 1);
            _splashUI.ShowBanner($"WAVE {waveNumber}", shouldSkipTypewriter);
        }
    }

    private void OnClickNextWave()
    {
        if (RoundManager.I != null && !RoundManager.I.IsWaveInProgress)
        {
            RoundManager.I.NextRound();
        }
    }
}
