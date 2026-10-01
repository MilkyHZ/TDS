using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager I;

    [SerializeField]
    private int _maxHp = 20;
    [SerializeField]
    private HealthUI _playerHpVisual;
    private float _currentHp;
    public float CurrentHp
    {
        get => _currentHp;
        set
        {
            if (_currentHp != value)
            {
                _currentHp = value;

                OnHealthChanged?.Invoke(_currentHp, _maxHp);
            }
        }
    }

    public event Action<float, float> OnHealthChanged;

    private void Start() => Init();

    public void Init()
    {
        I = this;
        _currentHp = _maxHp;
    }

    public void TakeDamage(int dmg = 1) => CurrentHp -= dmg;
}