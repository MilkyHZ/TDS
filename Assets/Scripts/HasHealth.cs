using System;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    [SerializeField]
    protected int _maxHp = 20;
    public int MaxHp => _maxHp;
    [SerializeField, ReadOnly]
    protected float _currentHp;
    public float CurrentHp
    {
        get => _currentHp;
        set
        {
            float clampedValue = Mathf.Clamp(value, 0, _maxHp);

            if (_currentHp != clampedValue)
            {
                _currentHp = clampedValue;
                OnHealthChanged?.Invoke(_currentHp, _maxHp);

                if (_currentHp <= 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }
    }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public void TakeDamage(int dmg = 1) => CurrentHp -= dmg;
    public void ResetHealth() => _currentHp = _maxHp;
}