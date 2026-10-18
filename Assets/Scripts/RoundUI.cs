using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    [Header("Wave Info")]
    [SerializeField] 
    private TextMeshProUGUI _txtWaveNumber;
    [SerializeField] 
    private TextMeshProUGUI _txtTotalEnemiesCount;

    [Header("Enemy Preview List Container")]
    [SerializeField] 
    private Transform _enemyCardsContainer;
    [SerializeField] 
    private EnemyPreviewCard _enemyCardPrefab;

    [Header("Controls")]
    [SerializeField]
     private Button _btnNextWave;

    private void Awake()
    {
        if (_btnNextWave != null)
        {
            _btnNextWave.onClick.AddListener(OnClickNextWave);
        }
    }

    private void OnEnable()
    {
        if (RoundManager.I != null)
        {
            RoundManager.I.OnRoundDataUpdated += UpdateRoundUI;
            if (RoundManager.I.CurrentRoundData != null)
            {
                UpdateRoundUI(RoundManager.I.TotalRounds + 1, RoundManager.I.CurrentRoundData);
            }
        }
    }

    private void OnDisable()
    {
        if (RoundManager.I != null)
        {
            RoundManager.I.OnRoundDataUpdated -= UpdateRoundUI;
        }
    }

    private void UpdateRoundUI(int waveNumber, RoundData roundData)
    {
        if (_txtWaveNumber != null)
        {
            _txtWaveNumber.text = $"WAVE {waveNumber}";
        }

        if (roundData == null) return;

        if (_txtTotalEnemiesCount != null)
        {
            _txtTotalEnemiesCount.text = $"Total Enemies: {roundData.GetTotalCreatureCount()}";
        }

        if (_enemyCardsContainer != null)
        {
            foreach (Transform child in _enemyCardsContainer)
            {
                Destroy(child.gameObject);
            }

            if (_enemyCardPrefab != null && roundData.Creatures != null)
            {
                foreach (var creature in roundData.Creatures)
                {
                    EnemyPreviewCard card = Instantiate(_enemyCardPrefab, _enemyCardsContainer);
                    card.Setup(creature.PreviewIcon, creature.SpawnCount);
                }
            }
        }

        if (_btnNextWave != null)
        {
            _btnNextWave.interactable = !RoundManager.I.IsWaveInProgress;
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