using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _radius = 0.5f;
    [SerializeField]
    private float _timeToReachTarget = 4f;
    [SerializeField]
    private CoinPopup _popupPrefab;

    private WalkPath _myPath;
    private float _totalTime;
    private Vector3 _initialPos;

    private void Update()
    {
        if (HealthManager.I.CurrentHp <= 0) return;

        if (_myPath.Target == null) return;

        _totalTime += Time.deltaTime;
        var lerpedTime = Mathf.Clamp01(_totalTime / _timeToReachTarget);

        if (_myPath.Type == WalkPath.PathType.Quadratic)
        {
            transform.position = Interpolation.QuadraticFast(
                _initialPos,
                _myPath.ControlPoints[0].position,
                _myPath.Target.position,
                lerpedTime
            );
        } 
        else if (_myPath.Type == WalkPath.PathType.Cubic)
        { 
            transform.position = Interpolation.CubicFast(
                _initialPos,
                _myPath.ControlPoints[0].position,
                _myPath.ControlPoints[1].position,
                _myPath.Target.position,
                lerpedTime
            );
        }

        CheckPlayerPosition();
    }

    public void InitializePath(WalkPath path)
    {
        _myPath = path;
        _initialPos = _myPath.transform.position;
        _totalTime = 0f;
    }

    private void CheckPlayerPosition() 
    {
        var dist = Vector3.Distance(transform.position, _myPath.Target.position);
        if (dist < _radius) 
        {
            HealthManager.I.TakeDamage(1);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Die() 
    {
        var spawnScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        gameObject.SetActive(false);

        var canvas = FindFirstObjectByType<Canvas>();
        var popup = Instantiate(_popupPrefab, canvas.transform);
        popup.SetSpawn(spawnScreenPos);
        Destroy(gameObject);
    }
}