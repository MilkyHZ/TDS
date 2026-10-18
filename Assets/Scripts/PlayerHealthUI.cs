using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _hpVisual;
    [SerializeField] private Image _ghostHpVisual;
    [SerializeField] private float _lerpSpeed = 3f;

    [SerializeField] private HasHealth _playerHealth;

    private float _targetFill = 1f;

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

        _targetFill = curHp / max;
        if (_hpVisual != null)
        {
            _hpVisual.fillAmount = _targetFill;
        }
    }
}