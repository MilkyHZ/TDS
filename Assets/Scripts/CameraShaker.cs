using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker I { get; private set; }

    private Vector3 _originalPos;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        I = this;
        _originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.2f, float magnitude = 0.3f)
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            transform.localPosition = _originalPos;
        }

        _shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = _originalPos + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}