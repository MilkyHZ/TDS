using UnityEngine;

public class WalkPath: MonoBehaviour
{
    public enum PathType { Quadratic, Cubic }

    [SerializeField]
    private PathType _pathType;
    public PathType Type => _pathType;

    [SerializeField]
    private Transform _target;
    public Transform Target => _target;

    [SerializeField]
    private Transform[] _controlPoints;
    public Transform[] ControlPoints 
    {
        get { return _controlPoints; }
        set { _controlPoints = value; }
    }

    [SerializeField] 
    private int _lineRes = 20;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        CreateVisualPath();
    }

    private void CreateVisualPath()
    {
        if (_lineRenderer == null || _controlPoints == null || _controlPoints.Length == 0 || _target == null) return;

        _lineRenderer.positionCount = _lineRes + 1;
        _lineRenderer.useWorldSpace = true;

        for (int i = 0; i <= _lineRes; i++)
        {
            float t = i / (float)_lineRes;
            _lineRenderer.SetPosition(i, GetPointOnPath(t));
        }
    }

    public Vector3 GetPointOnPath(float t)
    {
        if (_controlPoints == null || _controlPoints.Length == 0 || _target == null) return transform.position;

        t = Mathf.Clamp01(t);
        Vector3 startPoint = transform.position;

        switch (_pathType)
        {
            case PathType.Quadratic:
                if (_controlPoints.Length < 1) return transform.position;
                return Mathf.Pow(1 - t, 2) * startPoint +
                       2 * (1 - t) * t * _controlPoints[0].position +
                       Mathf.Pow(t, 2) * _target.position;

            case PathType.Cubic:
                if (_controlPoints.Length < 2) return transform.position;
                return Mathf.Pow(1 - t, 3) * startPoint +
                       3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[0].position +
                       3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[1].position +
                       Mathf.Pow(t, 3) * _target.position;
            default:
                return transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (_target == null) return;

        Gizmos.color = Color.red;
        Vector3 previousPoint = GetPointOnPath(0);

        for (int i = 1; i <= _lineRes; i++)
        {
            float t = i / (float)_lineRes;
            Vector3 currentPoint = GetPointOnPath(t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}