using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private Image _hpVisual;
    [SerializeField]
    private Image _ghostHpVisual;
    [SerializeField]
    private float _lerpSpeed = 3f;

    [Header("Damage Flash")]
    [SerializeField]
    private Image _redDamageOverlay;
    [SerializeField]
    private float _flashSpeed = 5f;

    [SerializeField]
    private HasHealth _playerHealth;

    private float _targetFill = 1f;
    private Coroutine _flashCoroutine;

    private void Start()
    {
        if (_playerHealth == null && PlayerHealthManager.I != null)
        {
            _playerHealth = PlayerHealthManager.I.GetComponent<HasHealth>();
        }

        BindHealthEvents();
    }

    private void OnEnable()
    {
        BindHealthEvents();
        RefreshUI();
    }

    private void OnDisable()
    {
        UnbindHealthEvents();
    }

    private void BindHealthEvents()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthVisual;
            _playerHealth.OnHealthChanged += UpdateHealthVisual;
            RefreshUI();
        }
    }

    private void UnbindHealthEvents()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthVisual;
        }
    }

    private void Update()
    {
        if (_ghostHpVisual != null && Mathf.Abs(_ghostHpVisual.fillAmount - _targetFill) > 0.001f)
        {
            _ghostHpVisual.fillAmount = Mathf.Lerp(
                _ghostHpVisual.fillAmount,
                _targetFill,
                Time.deltaTime * _lerpSpeed
            );
        }
    }

    public void RefreshUI()
    {
        if (_playerHealth == null || _playerHealth.MaxHp <= 0) return;

        _targetFill = (float)_playerHealth.CurrentHp / _playerHealth.MaxHp;

        if (_hpVisual != null) _hpVisual.fillAmount = _targetFill;
        if (_ghostHpVisual != null) _ghostHpVisual.fillAmount = _targetFill;
    }

    private void UpdateHealthVisual(float curHp, float max)
    {
        if (max <= 0) return;

        float newFill = curHp / max;

        // Flash red overlay if HP decreased
        if (newFill < _targetFill)
        {
            TriggerRedFlash();
        }

        _targetFill = newFill;
        if (_hpVisual != null)
        {
            _hpVisual.fillAmount = _targetFill;
        }
    }

    private void TriggerRedFlash()
    {
        if (_redDamageOverlay == null) return;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(RedFlashRoutine());
    }

    private IEnumerator RedFlashRoutine()
    {
        Color c = _redDamageOverlay.color;
        c.a = 0.6f;
        _redDamageOverlay.color = c;

        while (c.a > 0.01f)
        {
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * _flashSpeed);
            _redDamageOverlay.color = c;
            yield return null;
        }

        c.a = 0f;
        _redDamageOverlay.color = c;
        _flashCoroutine = null;
    }
}