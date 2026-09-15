using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _collectible;
    [SerializeField]
    private MeshRenderer _plane;
    [SerializeField]
    private float _yAxis = 0.1f;

    private float _xDim;
    private float _zDim;

    [SerializeField]
    private float _interval = 5f;

    private float _spawnTimer;

    private void Start()
    {
        _xDim = _plane.bounds.extents.x;
        _zDim = _plane.bounds.extents.z;
    }

    public void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _interval;
            Spawn();
        }
    }

    private void Spawn()
    {
        var obj = Instantiate(_collectible, Vector3.zero, Quaternion.identity);

        var xRand = Random.Range(-_xDim, _xDim);
        var zRand = Random.Range(-_zDim, _zDim);

        obj.transform.position = new Vector3(xRand, _yAxis, zRand);

        obj.transform.parent = null;
    }
}
