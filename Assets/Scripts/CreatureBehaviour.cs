using System.Collections;
using UnityEngine;

public class CreatureBehaviour : HasPool<CreatureBehaviour>
{
    [Header("Movement Settings")]
    [SerializeField]
    private float _moveSpeed = 3f;
    public float MoveSpeed => _moveSpeed;

    [SerializeField]
    private float _impactRadius = 0.5f;

    [Header("Rewards & Visuals")]
    [SerializeField]
    private int _goldReward = 5;
    [SerializeField]
    private CoinPopup _coinPopupPrefab;

    [Header("Combat Settings")]
    [SerializeField]
    private int _damageDealt = 1;

    private HasHealth _health;
    public HasHealth Health => _health;

    private WalkPath _myPath;
    private float _currentDistanceTraveled;
    private float _pathLength;
    private Canvas _worldCanvas;
    private Camera _mainCamera;

    private float _speedMultiplier = 1f;
    private float _roundSpeedMultiplier = 1f;
    private Coroutine _slowCoroutine;

    private bool _isReachingBase = false;

    private void Awake()
    {
        _health = GetComponent<HasHealth>();
    }

    private void OnEnable()
    {
        _currentDistanceTraveled = 0f;
        _speedMultiplier = 1f;
        _roundSpeedMultiplier = 1f;
        _isReachingBase = false;

        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
            _slowCoroutine = null;
        }

        if (_health != null)
        {
            _health.ResetHealth();
            _health.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDeath -= HandleDeath;
        }
    }

    public void InitializePath(WalkPath path, Canvas canvas, Camera mainCam)
    {
        _myPath = path;
        _worldCanvas = canvas;
        _mainCamera = mainCam;
        _currentDistanceTraveled = 0f;

        _pathLength = _myPath != null ? _myPath.GetPathLength() : 1f;

        int currentRound = RoundManager.I != null ? RoundManager.I.TotalRounds + 1 : 1;

        _roundSpeedMultiplier = 1f + (Mathf.Max(0, currentRound - 1) * 0.1f);
    }

    private void Update()
    {
        if (_health != null && _health.CurrentHp <= 0) return;
        if (_myPath == null || _myPath.Target == null) return;

        float effectiveSpeed = _moveSpeed * _speedMultiplier * _roundSpeedMultiplier;
        _currentDistanceTraveled += effectiveSpeed * Time.deltaTime;

        float progress = Mathf.Clamp01(_currentDistanceTraveled / _pathLength);
        transform.position = _myPath.GetPointOnPath(progress);

        CheckReachedBase();
    }

    private void CheckReachedBase()
    {
        float distToBase = Vector3.Distance(transform.position, _myPath.Target.position);
        float targetRadius = FortressLayerManager.I != null ? FortressLayerManager.I.CurrentRadius : _impactRadius;

        if (distToBase <= targetRadius)
        {
            var activeWall = FortressLayerManager.I != null ? FortressLayerManager.I.GetActiveWall() : null;
            if (activeWall != null)
            {
                activeWall.TakeDamage(_damageDealt);
            }

            if (_health != null)
            {
                _isReachingBase = true;
                _health.TakeDamage((int)_health.MaxHp);
            }
            else
            {
                Release(this);
            }
        }
    }

    public void HandleDeath()
    {
        if (!_isReachingBase)
        {
            if (_mainCamera != null && _worldCanvas != null && _coinPopupPrefab != null)
            {
                var spawnScreenPos = _mainCamera.WorldToScreenPoint(transform.position);
                var popup = GameplayFactory.I.GetEntity(_coinPopupPrefab, _worldCanvas.transform, spawnScreenPos);
                popup.SetSpawn(spawnScreenPos).SetValue(_goldReward);
            }
        }

        Release(this);
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
        }
        _slowCoroutine = StartCoroutine(SlowRoutine(slowFactor, duration));
    }

    private IEnumerator SlowRoutine(float slowFactor, float duration)
    {
        _speedMultiplier = Mathf.Clamp01(slowFactor);

        yield return new WaitForSeconds(duration);

        _speedMultiplier = 1f;
        _slowCoroutine = null;
    }
}