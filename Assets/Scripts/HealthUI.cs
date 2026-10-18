using UnityEngine;
using UnityEngine.UI;

public class HealthUI : HasPool<HealthUI>
{
    [SerializeField] 
    private Image _hpVisual;
    [SerializeField]
    private Image _ghostHpVisual;
    [SerializeField]
    private float _lerpSpeed = 3f;
    [SerializeField]
    private Vector3 _offset = new(0, 2.5f, 0);

    [SerializeField] 
    private HasHealth _targetHealth;
    private float _targetFill;

    [SerializeField] 
    private bool _followTarget = true;

    public void SetTarget(HasHealth health)
    {
        if (_targetHealth != null)
        {
            _targetHealth.OnHealthChanged -= UpdateHealthVisual;
            _targetHealth.OnDeath -= HandleTargetDeath;
        }

        _targetHealth = health;

        if (_targetHealth != null)
        {
            _targetHealth.OnHealthChanged += UpdateHealthVisual;
            _targetHealth.OnDeath += HandleTargetDeath;

            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (_targetHealth == null || _targetHealth.MaxHp <= 0) return;

        _targetFill = (float)_targetHealth.CurrentHp / _targetHealth.MaxHp;

        if (_hpVisual != null) _hpVisual.fillAmount = _targetFill;
        if (_ghostHpVisual != null) _ghostHpVisual.fillAmount = _targetFill;
    }

    private void OnDisable()
    {
        if (_targetHealth != null)
        {
            _targetHealth.OnHealthChanged -= UpdateHealthVisual;
            _targetHealth.OnDeath -= HandleTargetDeath;
            _targetHealth = null;
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

    private void LateUpdate()
    {
        if (!_followTarget || _targetHealth == null) return;
        transform.position = _targetHealth.transform.position + _offset;

        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
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

    private void HandleTargetDeath() => Release(this);
}