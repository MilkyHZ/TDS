using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager I;

    public float RoundTime = 3f;
    public int TotalRounds = -1;

    [SerializeField]
    private float _roundMultiplier = 0.2f;

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

    [SerializeField]
    private RoundData[] _roundData;
    public RoundData[] RoundData => _roundData;
    public RoundData CurrentRoundData;

    public int MaxRounds = 10;

    [SerializeField]
    private CreatureSpawner _spawner;

    public bool IsWaveInProgress { get; private set; } = false;

    public event Action<int, RoundData> OnRoundDataUpdated;
    public event Action<int> OnWaveStarted;

    private void Awake() => I = this;

    public void ResetRounds()
    {
        CancelInvoke(nameof(NextRound));

        if (_spawner != null)
        {
            _spawner.StopAndResetSpawner();
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

        OnWaveStarted?.Invoke(TotalRounds + 1);
        OnRoundDataUpdated?.Invoke(TotalRounds + 1, CurrentRoundData);

        if (_spawner != null && CurrentRoundData != null)
        {
            _spawner.StartSpawning(CurrentRoundData);
        }
    }

    public void OnRoundCleared()
    {
        if (!IsWaveInProgress) return;

        IsWaveInProgress = false;
        OnRoundDataUpdated?.Invoke(TotalRounds + 1, CurrentRoundData);

        if (TotalRounds + 1 >= MaxRounds)
        {
            if (GameControler.I != null)
            {
                GameControler.I.WinGame();
            }
            return;
        }

        Invoke(nameof(NextRound), RoundTime);
    }
}