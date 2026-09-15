using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 1f;
    [SerializeField]
    private float _speed = 1f;
    
    private Vector3 _direction;
    private float _timeLeft;

    private void Start() => _timeLeft = _lifeTime;

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0) 
            Destroy(gameObject);
        
        transform.position += _direction * (_speed * Time.deltaTime);    
    }

    public void SetDirection(Vector3 dir) => _direction = dir;
}
