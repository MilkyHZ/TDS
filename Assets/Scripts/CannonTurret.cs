using UnityEngine;

public class CannonTurret : BaseTurret
{
    [Header("Cannon Splash Settings")]
    [SerializeField]
    private float _bulletSpeed = 50f;
    [SerializeField]
    private float _hitRadius = 1.2f;
    [SerializeField] 
    private int _damage = 5;
    [SerializeField] 
    private int _pierceAmount = 3;

    protected override void InitVisual() => DrawCircle();
    protected override bool IsTargetInDetectionZone() => _dist <= _range;

    public override void UpgradeToLevelMax()
    {
        base.UpgradeToLevelMax();
        _damage += 5;
        _pierceAmount += 2;
    }

    protected override void FireWeapon()
    {
        if (_bulletPrefab == null || _firePoint == null) return;

        if (_bulletPrefab.TryGetComponent<TurretBulletBehaviour>(out var bulletComponent))
        {
            TurretBulletBehaviour bullet = GameplayFactory.I.GetEntity(
                bulletComponent,
                _firePoint.position,
                _head != null ? _head.rotation : Quaternion.identity
            );

            bullet.Speed = _bulletSpeed;
            bullet.HitRadius = _hitRadius;
            bullet.Damage = _damage;
            bullet.PierceAmount = _pierceAmount;
        }
    }
}