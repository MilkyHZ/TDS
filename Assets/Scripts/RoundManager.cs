using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager I;

    public float RoundTime = 3f;
    public int TotalRounds = -1;

    [SerializeField] private float _roundMultiplier = 0.2f;

    private float _currentMultiplier;
    public float RoundMultiplier => _currentMultiplier;

    private int _currentRound = -1;
    public int CurrentRound
    {
        get => _currentRound;
        set
        {
            if (RoundData != null && RoundData.Length > 0)
            {
                int rawIndex = value % RoundData.Length;
                _currentRound = rawIndex < 0 ? rawIndex + RoundData.Length : rawIndex;
                CurrentRoundData = RoundData[_currentRound];

                var loopCount = TotalRounds / RoundData.Length;
                _currentMultiplier = 1f + (loopCount * _roundMultiplier);
            }
            else
            {
                _currentRound = value;
            }
        }
    }

    [SerializeField] private RoundData[] _roundData;
    public RoundData[] RoundData => _roundData;
    public RoundData CurrentRoundData;

    [SerializeField] private CreatureSpawner _spawner;

    public bool IsWaveInProgress { get; private set; } = false;

    public event Action<int, RoundData> OnRoundDataUpdated;

    private void Awake() => I = this;

    private void Start() => NextRound();

    public void ResetRounds()
    {
        CancelInvoke(nameof(NextRound));

        if (_spawner != null)
        {
            _spawner.StopAllCoroutines();
        }

        IsWaveInProgress = false;
        TotalRounds = -1;
        CurrentRound = -1;

        NextRound();
    }

    public void NextRound()
    {
        CancelInvoke(nameof(NextRound));

        TotalRounds++;
        CurrentRound++;

        IsWaveInProgress = true;
        OnRoundDataUpdated?.Invoke(TotalRounds + 1, CurrentRoundData);

        if (_spawner != null && CurrentRoundData != null)
        {
            _spawner.StartSpawning(CurrentRoundData);
        }
    }

    public void OnRoundCleared()
    {
        IsWaveInProgress = false;
        OnRoundDataUpdated?.Invoke(TotalRounds + 1, CurrentRoundData);

        Invoke(nameof(NextRound), RoundTime);
    }
}