using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoGoZone : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [Header("Proximity")]
    [SerializeField]
    private float _farZone = 5f;

    [SerializeField]
    private float _nearZone = 2f;

    [SerializeField]
    private float _timer = 3f;

    [Header("Visual Control")]
    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private float _intensity = 0.1f;

    [SerializeField]
    private Image _warningImage;

    private float _zoneTimer;
    private Vector3 _originalCameraPosition;

    private void Start()
    {
        _originalCameraPosition = _cam.transform.localPosition;
        _warningImage.color = new (1f, 0f, 0f, 0f);
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, _player.position);

        if (dist <= _nearZone)
        {
            _zoneTimer += Time.deltaTime;

            CameraShake();
            UpdateWarningImage(0.6f);

            if (_zoneTimer >= _timer)
            {
                RestartScene();
            }
        }

        else if (dist <= _farZone)
        {
            _zoneTimer = 0f;

            CameraShake();
            UpdateWarningImage(0.3f);
        }

        else
        {
            _zoneTimer = 0f;

            ResetVisuals();
        }
    }

    private void CameraShake()
    {
        Vector3 randomOffset = Random.insideUnitSphere * _intensity;
        _cam.transform.localPosition = _originalCameraPosition + randomOffset;
    }

    private void UpdateWarningImage(float alpha)
    {
        _warningImage.color = new Color(1f, 0f, 0f, alpha);
    }

    private void ResetVisuals()
    {
        _cam.transform.localPosition = _originalCameraPosition;
        _warningImage.color = new Color(1f, 0f, 0f, 0f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
