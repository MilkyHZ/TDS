using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Image _hpVisual;
    [SerializeField]
    private Image _ghostHpVisual;
    [SerializeField]
    private float _lerpSpeed = 3f;

    private float _targetFill;

    private void Start() => HealthManager.I.OnHealthChanged += UpdateHealthVisual;
    
    private void OnDestroy() => HealthManager.I.OnHealthChanged -= UpdateHealthVisual;

    private void Update()
    {
        if (_ghostHpVisual.fillAmount != _targetFill)
        {
            _ghostHpVisual.fillAmount = Mathf.Lerp(
                _ghostHpVisual.fillAmount,
                _targetFill,
                Time.deltaTime * _lerpSpeed
                );
        }
    }

    private void UpdateHealthVisual(float curHp, float max) 
    {
        _targetFill =  curHp / max;
        _hpVisual.fillAmount = _targetFill;
    }
}