using UnityEngine;

public class FlameTurret : BaseTurret
{
    [SerializeField]
    private float _bulletSpd = 2f;

    protected override void InitVisual() => DrawCone();
    protected override bool IsTargetInDetectionZone() => IsTargetInCone();
    protected override void FireWeapon()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        var bulletScript = bullet.GetComponent<TurretBulletBehaviour>();
        bulletScript.Speed = _bulletSpd;
    }
}