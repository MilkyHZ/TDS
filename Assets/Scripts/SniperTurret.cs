using UnityEngine;

public class SniperTurret : BaseTurret
{
    [Header("Sniper Settings")]
    [SerializeField]
    private float _bulletSpd = 250f;
    [SerializeField]
    private float _alignment = 0.98f;
    [SerializeField]
    private float _killRadius = 2f;

    public override void Start() 
    {
        base.Start();
        _targetFollow = false;
    }
    protected override bool CanFireWeapon() => _alignmentDot >= _alignment;
    protected override void InitVisual() => DrawCone();
    protected override bool IsTargetInDetectionZone() => IsTargetInCone();
    protected override void FireWeapon()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        var bulletScript = bullet.GetComponent<TurretBulletBehaviour>();
        bulletScript.Speed = _bulletSpd;
        bulletScript.KillRadius = _killRadius;
    }
}