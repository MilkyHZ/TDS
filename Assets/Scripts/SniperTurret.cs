using UnityEngine;

public class SniperTurret : BaseTurret
{
    [SerializeField]
    private float _bulletSpd = 250f;
    [SerializeField]
    private float _alignment = 0.98f;

    protected override bool CanFireWeapon() => _alignmentDot >= _alignment;
    protected override void InitVisual() => DrawCone();
    protected override bool IsTargetInDetectionZone() => IsTargetInCone();
    protected override void FireWeapon()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        var bulletScript = bullet.GetComponent<TurretBulletBehaviour>();
        bulletScript.Speed = _bulletSpd;
    }
}