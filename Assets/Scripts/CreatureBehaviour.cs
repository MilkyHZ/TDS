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

    [Header("Combat Settings")]
    [SerializeField]
    private int _damageDealt = 1;

    [Header("Rewards & Visuals")]
    [SerializeField]
    private int _goldReward = 5;
    [SerializeField]
    private Sprite _previewIcon;
    public Sprite PreviewIcon => _previewIcon;
    [SerializeField]
    private CoinPopup _coinPopupPrefab;

    [Header("Animation Settings")]
    [SerializeField] 
    private Animator _animator;
    [SerializeField] 
    private string _isWalkingBoolParam = "IsWalking";
    [SerializeField] 
    private string _moveSpeedFloatParam = "MoveSpeed";

    [Header("Hit Visual Settings")]
    [SerializeField] 
    private Texture2D _mainTexture; 
    [SerializeField] 
    private Color _hitFlashColor = Color.white;
    [SerializeField] 
    private float _flashDuration = 0.1f;
    [SerializeField]
    private ParticleObject _hitParticlePrefab;

    private static readonly int MainTexID = Shader.PropertyToID("_Main_Texture");
    private static readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashColorID = Shader.PropertyToID("_FlashColor");

    private HasHealth _health;
    public HasHealth Health => _health;

    private WalkPath _myPath;
    private float _currentDistanceTraveled;
    private float _pathLength;
    private Canvas _worldCanvas;
    private Canvas _canvas;
    private Camera _mainCamera;

    private float _speedMultiplier = 1f;
    private float _roundSpeedMultiplier = 1f;
    private Coroutine _slowCoroutine;
    private Coroutine _flashCoroutine;

    private Renderer[] _renderers;
    private MaterialPropertyBlock _propertyBlock;

    private bool _isReachingBase = false;

    private void Awake()
    {
        _health = GetComponent<HasHealth>();

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>(true);
        }

        _renderers = GetComponentsInChildren<Renderer>(true);
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        _canvas = canvas.GetComponent<Canvas>();

        _currentDistanceTraveled = 0f;
        _speedMultiplier = 1f;
        _roundSpeedMultiplier = 1f;
        _isReachingBase = false;

        ResetFlash();

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>(true);
        }

        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
            SetWalkingAnimation(true, _moveSpeed);
        }

        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
            _slowCoroutine = null;
        }

        if (_health != null)
        {
            _health.ResetHealth();
            _health.OnDeath += HandleDeath;
            _health.OnHealthChanged += OnDamaged;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDeath -= HandleDeath;
            _health.OnHealthChanged -= OnDamaged;
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
        if (_health != null && _health.CurrentHp <= 0)
        {
            SetWalkingAnimation(false);
            return;
        }
        if (_myPath == null || _myPath.Target == null)
        {
            SetWalkingAnimation(false);
            return;
        }

        float effectiveSpeed = _moveSpeed * _speedMultiplier * _roundSpeedMultiplier;
        Vector3 previousPosition = transform.position;

        _currentDistanceTraveled += effectiveSpeed * Time.deltaTime;

        float progress = Mathf.Clamp01(_currentDistanceTraveled / _pathLength);
        Vector3 targetPosition = _myPath.GetPointOnPath(progress);

        transform.position = targetPosition;

        Vector3 moveDirection = targetPosition - previousPosition;
        moveDirection.y = 0;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        SetWalkingAnimation(progress < 1.0f, effectiveSpeed);

        CheckReachedBase();
    }

    private void SetWalkingAnimation(bool isWalking, float speed = 0f)
    {
        if (_animator == null) return;

        if (!string.IsNullOrEmpty(_isWalkingBoolParam))
        {
            _animator.SetBool(_isWalkingBoolParam, isWalking);
        }

        if (!string.IsNullOrEmpty(_moveSpeedFloatParam))
        {
            _animator.SetFloat(_moveSpeedFloatParam, speed);
        }
    }

    private void OnDamaged(float currentHp, float maxHp)
    {
        if (currentHp <= 0) return;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        _flashCoroutine = StartCoroutine(HitFlashRoutine());

        GameplayFactory.I.GetEntity(_hitParticlePrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator HitFlashRoutine()
    {
        ApplyFlashProperties(1f); 

        yield return new WaitForSeconds(_flashDuration);

        ResetFlash();
        _flashCoroutine = null;
    }

    private void ResetFlash()
    {
        ApplyFlashProperties(0f);
    }

    private void ApplyFlashProperties(float flashAmount)
    {
        if (_renderers == null || _propertyBlock == null) return;

        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] == null) continue;

            _renderers[i].GetPropertyBlock(_propertyBlock);

            if (_mainTexture != null)
            {
                _propertyBlock.SetTexture(MainTexID, _mainTexture);
            }

            _propertyBlock.SetColor(FlashColorID, _hitFlashColor);
            _propertyBlock.SetFloat(FlashAmountID, flashAmount);

            _renderers[i].SetPropertyBlock(_propertyBlock);
        }
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
                _health.TakeDamage(_health.MaxHp);
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
                var popup = GameplayFactory.I.GetEntity(_coinPopupPrefab, _canvas.transform, spawnScreenPos);
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