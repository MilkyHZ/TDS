using UnityEngine;

public class SlowTurret : BaseTurret
{
    [Header("Frost / Slow Settings")]
    [SerializeField]
    private float _bulletSpeed = 50f;
    [SerializeField] 
    private int _damage = 1;

    [Header("AoE Debuff Tweaks")]
    [SerializeField] 
    private float _slowMultiplier = 0.5f;
    [SerializeField] 
    private float _slowDuration = 2.0f;

    protected override void InitVisual() => DrawCircle();

    protected override bool IsTargetInDetectionZone() => _dist <= _range;

    protected override bool CanFireWeapon() => true;

    public override void UpgradeToLevelMax()
    {
        base.UpgradeToLevelMax();
        _damage += 1;
        _slowDuration += 1.0f;
    }

    protected override void OnFire()
    {
        CreatureBehaviour[] allCreatures = FindObjectsByType<CreatureBehaviour>(FindObjectsSortMode.None);

        bool hitAnyEnemy = false;

        foreach (CreatureBehaviour creature in allCreatures)
        {
            if (creature == null) continue;

            float distanceToCreature = Vector3.Distance(transform.position, creature.transform.position);
            if (distanceToCreature <= _range)
            {
                hitAnyEnemy = true;

                creature.ApplySlow(_slowMultiplier, _slowDuration);

                if (creature.Health != null)
                {
                    creature.Health.TakeDamage(_damage);
                }
            }
        }


        if (hitAnyEnemy && _bulletPrefab != null && _firePoint != null)
        {
            Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        }

    }
}
