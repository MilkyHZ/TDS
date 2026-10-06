using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _misslePrefab;
    [SerializeField]
    private Transform _planeTransform;
    [SerializeField]
    private int _maxX = 10;
    [SerializeField]
    private float _spawnInterval = 1;
    [SerializeField]
    private float _incrementTime = 10f; 

    private float _spawnCount = 1;
    private float _intervalTimer;
    private float _incrementTimer;

    private void Update()
    {
        _intervalTimer += Time.deltaTime;
        _incrementTimer += Time.deltaTime;

        if (_intervalTimer >= _spawnInterval)
        {
            _intervalTimer = 0f;

            for (int i = 0; i < _spawnCount; i++) 
            {
                Vector3 pos = new(Random.Range(-_maxX, _maxX), transform.position.y , transform.position.z);
                FireMissle(pos);
            }
        }

        if (_incrementTimer >= _incrementTime)
        {
            _incrementTimer = 0;
            _spawnCount++;
        }
}

    private void FireMissle(Vector3 pos)
    {
        var missle = Instantiate(_misslePrefab, pos, transform.rotation);
        var missleScript = missle.GetComponent<HomingMissileBehaviour>();
        missleScript.Target = _planeTransform;
    }
}
