using System.Collections;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public static MovingFloor I { get; private set; }

    [SerializeField]
    private Vector3 _startPosition = new(0f, 0f, -28.4f);

    [SerializeField]
    private Vector3 _targetPosition = new(0f, 0f, 0f);

    [SerializeField]
    private float _moveDuration = 1.2f;

    private Coroutine _moveCoroutine;

    private void Awake()
    {
        I = this;
        transform.localPosition = _startPosition;
    }

    public void MoveToGameplayPosition()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveRoutine(_targetPosition));
    }

    public void MoveToTitlePosition()
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveRoutine(_startPosition));
    }

    private IEnumerator MoveRoutine(Vector3 destination)
    {
        Vector3 start = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < _moveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / _moveDuration);
            transform.localPosition = Vector3.Lerp(start, destination, t);
            yield return null;
        }

        transform.localPosition = destination;
        _moveCoroutine = null;
    }
}