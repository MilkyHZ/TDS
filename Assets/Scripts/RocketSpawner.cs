using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField]
    private RocketBehaviour _rocketPrefab;
    [SerializeField]
    private int _rocketCount = 1;
    [SerializeField]
    private float _maxRockets = 8;

    [SerializeField]
    private float _interval = 3f;

    private float _spawnTimer;

    public void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _interval;
            Shoot();
        }
    }

    private void Shoot() 
    {
        for (int i = 0; i < _rocketCount; i++) 
        {
            var rocket = Instantiate(_rocketPrefab, transform.position, Quaternion.identity);
            rocket.SetDirection(GetDirection(i));
        }
    }

    public Vector3 GetDirection(int index)
    {
        float angleSpacing = 360 / _rocketCount;
        float firstOffset = angleSpacing / 2f;

        float currentAngle = firstOffset + (index * angleSpacing);

        float radian = currentAngle * Mathf.Deg2Rad;

        float x = Mathf.Cos(radian);
        float z = Mathf.Sin(radian);

        return new Vector3(x, 0f, z);
    }

    public void IncreaseRocketCount() 
    {
        if (!(_rocketCount >= _maxRockets))
            _rocketCount++;
    }
}
