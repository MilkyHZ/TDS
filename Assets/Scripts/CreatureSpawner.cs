using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField]
    private WalkPath[] _walkPaths;
    [SerializeField]
    private GameObject[] _creature;
    [SerializeField]
    private float _interval;
    
    private float _spawnTimer;

    private void Start() => _spawnTimer = _interval;

    private void Update()
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
        if (_creature == null || _walkPaths == null || _walkPaths.Length == 0) return;

        var randCreature = _creature[Random.Range(0, _creature.Length)];
        var randPath = _walkPaths[Random.Range(0, _walkPaths.Length)];

        var creature = Instantiate(randCreature, randPath.transform);
        var creatureScript = creature.GetComponent<CreatureBehaviour>();

        var chosenPath = randPath;
        creatureScript.InitializePath(chosenPath);
    }
}