using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] 
    private WalkPath[] _walkPaths;
    [SerializeField]
    private Canvas _worldCanvas;
    [SerializeField] 
    private Camera _mainCamera;
    [SerializeField] 
    private HealthUI _healthUIPrefab;
    [SerializeField] 
    private float _spawnInterval = 0.5f;

    private bool _isSpawning = false;
    public bool IsSpawning => _isSpawning;

    private int _activeCreatureCount = 0;
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }

    public void StopAndResetSpawner()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
        _isSpawning = false;
        _activeCreatureCount = 0;
    }

    public void StartSpawning(RoundData roundData)
    {
        if (roundData == null) return;

        StopAndResetSpawner();
        _spawnCoroutine = StartCoroutine(SpawnRoundRoutine(roundData));
    }

    private IEnumerator SpawnRoundRoutine(RoundData roundData)
    {
        _isSpawning = true;
        _activeCreatureCount = 0;

        List<CreatureBehaviour> spawnQueue = new();

        foreach (var spawnData in roundData.Creatures)
        {
            if (spawnData.Type == null) continue;

            for (int i = 0; i < spawnData.SpawnCount; i++)
            {
                spawnQueue.Add(spawnData.Type);
            }
        }

        for (int i = 0; i < spawnQueue.Count; i++)
        {
            var temp = spawnQueue[i];
            int randIndex = Random.Range(i, spawnQueue.Count);
            spawnQueue[i] = spawnQueue[randIndex];
            spawnQueue[randIndex] = temp;
        }

        foreach (var enemyPrefab in spawnQueue)
        {
            if (_walkPaths == null || _walkPaths.Length == 0) break;

            WalkPath chosenPath = _walkPaths[Random.Range(0, _walkPaths.Length)];

            CreatureBehaviour creature = GameplayFactory.I.GetEntity(
                enemyPrefab,
                chosenPath.transform.position,
                Quaternion.identity
            );

            _activeCreatureCount++;

            creature.Health.OnDeath -= OnCreatureDespawned;
            creature.Health.OnDeath += OnCreatureDespawned;

            creature.InitializePath(chosenPath, _worldCanvas, _mainCamera);

            if (_healthUIPrefab != null && _worldCanvas != null)
            {
                Vector2 spawnScreenPos = _mainCamera.WorldToScreenPoint(creature.transform.position);

                HealthUI healthUI = GameplayFactory.I.GetEntity(
                    _healthUIPrefab,
                    _worldCanvas.transform,
                    spawnScreenPos
                );

                healthUI.SetTarget(creature.Health);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }

        _isSpawning = false;

        CheckWaveCompletion();
    }

    private void OnCreatureDespawned()
    {
        _activeCreatureCount--;
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if (_activeCreatureCount <= 0 && !_isSpawning)
        {
            _activeCreatureCount = 0;
            if (RoundManager.I != null)
            {
                RoundManager.I.OnRoundCleared();
            }
        }
    }
}