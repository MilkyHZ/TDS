using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotationSpeed = 5f;
    [SerializeField]
    private float _tiltAngle = 35f;

    private float _currentYaw;

    private void Start() => _currentYaw = transform.eulerAngles.y;

    private void Update()
    {
        transform.position += transform.forward * (_speed * Time.deltaTime);

        float x = Input.GetAxis("Horizontal");
        _currentYaw += x * _rotationSpeed * Time.deltaTime;

        float targetZRotation = -_tiltAngle * x;

        Quaternion targetRotation = Quaternion.Euler(0f, _currentYaw, targetZRotation);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }


}
