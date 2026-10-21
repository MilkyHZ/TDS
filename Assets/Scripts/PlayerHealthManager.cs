using UnityEngine;

public class PlayerHealthManager : WallLayer
{
    public static PlayerHealthManager I;

    [Header("Juice FX")]
    [SerializeField]
    private float _shakeDuration = 0.25f;
    [SerializeField]
    private float _shakeMagnitude = 0.35f;

    private void Awake() => I = this;

    protected override void Start()
    {
        base.Start();

        OnDeath -= HandleDeath;
        OnDeath += HandleDeath;

        ResetHealth();
    }

    public override void TakeDamage(int dmg = 1)
    {
        base.TakeDamage(dmg);

        if (CameraShaker.I != null)
        {
            CameraShaker.I.Shake(_shakeDuration, _shakeMagnitude);
        }
    }

    public new void ResetHealth()
    {
        gameObject.SetActive(true);
        enabled = true;

        base.ResetHealth();
    }

    private void HandleDeath()
    {
        if (GameControler.I != null)
        {
            GameControler.I.GameOver();
        }
    }

    protected override void HandleWallBreak()
    {
        if (GameControler.I != null)
        {
            GameControler.I.GameOver();
        }
    }

    private void Upgrade(int amount = 1)
    {
        _maxHp += amount;
        CurrentHp += amount;
    }
}