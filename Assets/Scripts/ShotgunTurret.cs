using UnityEngine;

public class ShotgunTurret : BaseTurret
{
    [SerializeField]
    private int _bulletCount = 8;
    [SerializeField]
    private float _bulletSpd = 100;

    protected override void InitVisual() => DrawCircle();
    protected override bool IsTargetInDetectionZone() => _dist <= _range;
    protected override void FireWeapon()
    {
        if (_bulletPrefab == null || _firePoint == null) return;

        float baseAngle = Mathf.Atan2(_firePoint.forward.x, _firePoint.forward.z) * Mathf.Rad2Deg;

        float halfAngle = _angle / 2f;

        for (int i = 0; i < _bulletCount; i++)
        {
            float progress = (float)i / (_bulletCount - 1);
            float offsetAngle = Mathf.Lerp(-halfAngle, halfAngle, progress);

            Quaternion pelletRotation = Quaternion.Euler(0, baseAngle + offsetAngle, 0);

            var bullet = Instantiate(_bulletPrefab, _firePoint.position, pelletRotation);
            var bulletScript = bullet.GetComponent<TurretBulletBehaviour>();
            if (bulletScript != null)
            {
                bulletScript.Speed = _bulletSpd;
            }
        }
    }
}