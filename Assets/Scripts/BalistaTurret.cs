using UnityEngine;

public class BallistaTurret : BaseTurret
{
    [Header("Ballista Settings")]
    [SerializeField]
    private float _bulletSpeed = 150f;
    [SerializeField]
    private float _hitRadius = .8f;
    [SerializeField] 
    private int _damage = 2;
    [SerializeField] 
    private int _pierceAmount = 3;

    protected override void InitVisual() => DrawCircle();
    protected override bool IsTargetInDetectionZone() => _dist <= _range;

    public override void UpgradeToLevelMax()
    {
        base.UpgradeToLevelMax();
        _damage += 3;          // High damage boost
        _pierceAmount += 2;    // Pierces +2 additional enemies
    }

    protected override void FireWeapon()
    {
        if (_bulletPrefab == null || _firePoint == null) return;

        if (_bulletPrefab.TryGetComponent<TurretBulletBehaviour>(out var bulletComponent))
        {
            TurretBulletBehaviour bullet = GameplayFactory.I.GetEntity(
                bulletComponent,
                _firePoint.position,
                _head.rotation
            );

            bullet.Speed = _bulletSpeed;
            bullet.HitRadius = _hitRadius;
            bullet.Damage = _damage;
            bullet.PierceAmount = _pierceAmount;
        }
    }
}