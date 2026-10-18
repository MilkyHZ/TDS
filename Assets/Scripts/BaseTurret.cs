using UnityEngine;

public abstract class BaseTurret : MonoBehaviour
{
    [Header("Detection Visual Settings")]
    [SerializeField]
    private GameObject _detection;
    private LineRenderer _rangeRend;
    [SerializeField]
    private int _stepCount = 32;

    [Header("Turret Settings")]
    [SerializeField]
    protected GameObject _bulletPrefab;
    [SerializeField]
    protected Transform _firePoint;
    [SerializeField]
    protected Transform _head;
    [SerializeField]
    protected float _range = 10f;
    [SerializeField]
    protected float _angle = 45f;
    [SerializeField]
    protected float _rotSpeed = 5f;
    [SerializeField]
    protected float _fireInterval = 1f;

    protected CreatureBehaviour _target;
    private Vector3 _dir;
    protected float _dist;
    private float _cooldownTimer;
    protected float _alignmentDot;
    protected bool _targetFollow = true;

    public virtual void Start()
    {
        if (_detection != null)
        {
            _rangeRend = _detection.GetComponent<LineRenderer>();
            if (_rangeRend != null)
            {
                _rangeRend.useWorldSpace = false;
            }
        }

        InitVisual();
    }

    public virtual void Update()
    {
        _target = FindNearestTarget();

        if (_target == null) return;

        _dist = Vector3.Distance(transform.position, _target.transform.position);

        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        if (IsTargetInDetectionZone())
        {
            if (_targetFollow)
            {
                RotateTowards();
            }

            AlignHead();

            if (_cooldownTimer <= 0 && CanFireWeapon())
            {
                FireWeapon();
                _cooldownTimer = _fireInterval;
            }
        }
    }

    // --- LEVEL 2 MAX UPGRADE METHOD ---
    public virtual void UpgradeToLevelMax()
    {
        // 1. Boost Base Stats
        _range *= 1.35f;           // +35% Attack Range
        _fireInterval *= 0.65f;    // +35% Attack Speed (35% faster interval)
        _rotSpeed *= 1.5f;         // +50% Rotation speed

        // 2. Redraw Detection Visuals
        InitVisual();
    }

    protected virtual bool CanFireWeapon() => _alignmentDot >= 0.95f;
    protected abstract void FireWeapon();
    protected abstract bool IsTargetInDetectionZone();
    protected abstract void InitVisual();

    private void AlignHead()
    {
        if (_head == null) return;

        Vector3 flatForward = _head.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        Vector3 flatTargetDir = _target.transform.position - _head.transform.position;
        flatTargetDir.y = 0;
        flatTargetDir.Normalize();

        _alignmentDot = Vector3.Dot(flatForward, flatTargetDir);
    }

    protected void RotateTowards()
    {
        if (_head == null) return;

        _dir = _target.transform.position - _head.transform.position;
        _dir.y = 0;

        if (_dir == Vector3.zero) return;

        var angle = Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;
        _head.transform.rotation = Quaternion.Slerp(
            _head.transform.rotation,
            Quaternion.Euler(0, angle, 0),
            _rotSpeed * Time.deltaTime
        );

        if (_detection != null)
        {
            _detection.transform.rotation = _head.transform.rotation;
        }
    }

    protected bool IsTargetInCone()
    {
        if (_target == null || _head == null) return false;

        Vector3 flatTurretPos = new(transform.position.x, 0, transform.position.z);
        Vector3 flatTargetPos = new(_target.transform.position.x, 0, _target.transform.position.z);
        float flatDist = Vector3.Distance(flatTurretPos, flatTargetPos);

        if (flatDist > _range) return false;

        Vector3 targetDir = flatTargetPos - flatTurretPos;
        Vector3 flatHeadForward = _head.forward;
        flatHeadForward.y = 0;

        float targetAngle = Vector3.Angle(flatHeadForward, targetDir.normalized);

        return targetAngle <= _angle / 2f;
    }

    private CreatureBehaviour FindNearestTarget()
    {
        CreatureBehaviour[] allCreatures = FindObjectsByType<CreatureBehaviour>(FindObjectsSortMode.None);
        CreatureBehaviour closestValid = null;
        float shortestDistance = _range;

        CreatureBehaviour originalTarget = _target;

        if (originalTarget == null || originalTarget.gameObject == null)
        {
            originalTarget = null;
        }

        foreach (CreatureBehaviour creature in allCreatures)
        {
            if (creature == null) continue;

            _target = creature;
            float evaluatedDist = Vector3.Distance(transform.position, creature.transform.position);

            if (IsTargetInDetectionZone())
            {
                if (evaluatedDist < shortestDistance)
                {
                    shortestDistance = evaluatedDist;
                    closestValid = creature;
                }
            }
        }

        _target = closestValid != null ? closestValid : originalTarget;
        return closestValid;
    }

    #region Draw Visual

    protected void DrawCircle()
    {
        if (_rangeRend == null) return;

        _rangeRend.positionCount = _stepCount;

        for (int curStep = 0; curStep < _stepCount; curStep++)
        {
            var prog = (float)curStep / _stepCount;
            var curRad = Mathf.PI * 2 * prog;

            var x = Mathf.Sin(curRad) * _range;
            var z = Mathf.Cos(curRad) * _range;

            _rangeRend.SetPosition(curStep, new Vector3(x, 0, z));
        }
    }

    protected void DrawCone()
    {
        if (_rangeRend == null) return;

        _rangeRend.positionCount = _stepCount + 2;
        _rangeRend.SetPosition(0, Vector3.zero);

        var coneRad = _angle * Mathf.Deg2Rad;

        for (int curStep = 0; curStep <= _stepCount; curStep++)
        {
            var prog = (float)curStep / _stepCount;
            var curRad = Mathf.Lerp(-coneRad / 2f, coneRad / 2f, prog);

            var x = Mathf.Sin(curRad) * _range;
            var z = Mathf.Cos(curRad) * _range;

            _rangeRend.SetPosition(curStep + 1, new Vector3(x, 0, z));
        }
    }

    #endregion
}