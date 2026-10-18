using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    [SerializeField] private List<Transform> _controlPoints = new();
    public List<Transform> ControlPoints => _controlPoints;

    [SerializeField] private int _lineRes = 30;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        CreateVisualPath();
    }

    public void CreateVisualPath()
    {
        if (_lineRenderer == null || _target == null) return;

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.startWidth = 0.3f;
        _lineRenderer.endWidth = 0.3f;

        if (_lineRenderer.sharedMaterial == null)
        {
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        _lineRenderer.positionCount = _lineRes + 1;

        for (int i = 0; i <= _lineRes; i++)
        {
            float t = i / (float)_lineRes;
            Vector3 point = GetPointOnPath(t);
            point.y += 0.05f;
            _lineRenderer.SetPosition(i, point);
        }
    }

    public Vector3 GetPointOnPath(float t)
    {
        if (_target == null) return transform.position;
        t = Mathf.Clamp01(t);

        int pointCount = (_controlPoints != null ? _controlPoints.Count : 0) + 2;
        Vector3[] allPoints = new Vector3[pointCount];

        allPoints[0] = transform.position;
        for (int i = 0; i < (_controlPoints != null ? _controlPoints.Count : 0); i++)
        {
            if (_controlPoints[i] != null)
                allPoints[i + 1] = _controlPoints[i].position;
            else
                allPoints[i + 1] = transform.position;
        }
        allPoints[pointCount - 1] = _target.position;

        float totalSegments = pointCount - 1;
        float scaledT = t * totalSegments;
        int currentSegment = Mathf.Min((int)scaledT, pointCount - 2);
        float segmentT = scaledT - currentSegment;

        return Vector3.Lerp(allPoints[currentSegment], allPoints[currentSegment + 1], segmentT);
    }

    [ContextMenu("Add Waypoint Point")]
    private void AddWaypointInEditor()
    {
        GameObject newPoint = new ($"Point_{_controlPoints.Count + 1}");
        newPoint.transform.SetParent(transform);

        Vector3 spawnOffset = transform.position + (Vector3.forward * (_controlPoints.Count + 1) * 2f);
        newPoint.transform.position = spawnOffset;

        _controlPoints.Add(newPoint.transform);
    }

    public float GetPathLength()
    {
        if (Target == null) return 1f;

        float dist = Vector3.Distance(transform.position, Target.position);
        return dist > 0f ? dist : 1f;
    }

    private void OnDrawGizmos()
    {
        if (_target == null) return;

        Gizmos.color = Color.cyan;
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