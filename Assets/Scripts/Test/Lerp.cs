using UnityEngine;

public class Lerp : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _controlPoint;
    [SerializeField]
    private float _timeToReachTarget = 4f;

    private float _totalTime;
    private Vector3 _initialPos;

    private void Start() => _initialPos = transform.position;

    void Update()
    {
        if (_target == null) return;
        
        _totalTime += Time.deltaTime;
        var lerpedTime = Mathf.Clamp01(_totalTime /_timeToReachTarget);

        //transform.position = Vector3.Lerp(_initialPos, _target.position, lerpedTime);
        //transform.position = _initialPos + (_target.position - _initialPos) * (lerpedTime * lerpedTime);
        //transform.position = _initialPos + (_target.position - _initialPos) * Mathf.Pow(lerpedTime, 2);
        //transform.position = transform.position + (_target.position - _initialPos) * lerpedTime; 

        transform.position = QuadraticFast(_initialPos, _controlPoint.position, _target.position, lerpedTime);
    }

    private void Reset()
    {
        transform.position = _initialPos;
        _totalTime = 0f;
    }

    private Vector3 QuadraticFast(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 +
            2f * u * t * p1 +
            t * t * p2;
    }

    public static Vector3 CubicFast(
        Vector3 p0, 
        Vector3 p1, 
        Vector3 p2, 
        Vector3 p3, 
        float t)
    {
        float u = 1f - t;
        return u * u * u * p0 +
            3 * u * u * t * p1 +
            3 * u * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 CubicTangent(
        Vector3 p0, 
        Vector3 p1,
        Vector3 p2,
        Vector3 p3,
        float t)
    {
        float u = 1f - t;
        return 3 * u * u * (p1 - p0) +
            6 * u * t * (p2 - p1) +
            3 * t * t * (p3 - p2);
    }

}