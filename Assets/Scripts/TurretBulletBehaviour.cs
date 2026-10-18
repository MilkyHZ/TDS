using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBulletBehaviour : HasPool<TurretBulletBehaviour>
{
    [SerializeField]
    private float _speed = 15f;
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    [SerializeField]
    private float _hitRadius = 0.8f;
    public float HitRadius
    {
        get => _hitRadius;
        set => _hitRadius = value;
    }

    [SerializeField]
    private int _dmg = 1;
    public int Damage
    {
        get => _dmg;
        set => _dmg = value;
    }

    [SerializeField]
    private int _pierceAmount = 1;
    public int PierceAmount
    {
        get => _pierceAmount;
        set => _pierceAmount = value;
    }

    [SerializeField]
    private float _lifeTime = 4f;

    private int _currentHits = 0;
    private readonly HashSet<CreatureBehaviour> _hitCreatures = new();
    private Coroutine _lifeTimerRoutine;

    private void OnEnable()
    {
        _currentHits = 0;
        _hitCreatures.Clear();

        if (_lifeTimerRoutine != null) StopCoroutine(_lifeTimerRoutine);
        _lifeTimerRoutine = StartCoroutine(DespawnRoutine());
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        Release(this);
    }

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

        CreatureBehaviour hitCreature = CheckNearestEnemy();
        if (hitCreature == null) return;

        HitCreature(hitCreature);
    }

    private CreatureBehaviour CheckNearestEnemy()
    {
        Vector3 bulletFlat = new(transform.position.x, 0, transform.position.z);
        CreatureBehaviour[] allCreatures = FindObjectsByType<CreatureBehaviour>(FindObjectsSortMode.None);

        foreach (CreatureBehaviour creature in allCreatures)
        {
            if (creature == null || _hitCreatures.Contains(creature)) continue;

            Vector3 targetFlat = new(creature.transform.position.x, 0, creature.transform.position.z);
            float dist = Vector3.Distance(bulletFlat, targetFlat);

            if (dist <= _hitRadius)
            {
                return creature;
            }
        }
        return null;
    }

    private void HitCreature(CreatureBehaviour hit)
    {
        _hitCreatures.Add(hit);

        if (hit.Health != null)
        {
            hit.Health.TakeDamage(_dmg);
        }

        _currentHits++;

        if (_currentHits >= _pierceAmount)
        {
            Release(this);
        }
    }
}