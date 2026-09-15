using UnityEngine;

public class Collectible_Rocket : MonoBehaviour, ICollectible
{
    [SerializeField]
    private float radius = 1;

    private RocketSpawner _rocketSpawner;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rocketSpawner = FindAnyObjectByType<RocketSpawner>();
    }

    public void Collect()
    {
        _rocketSpawner.IncreaseRocketCount();    
        Destroy(gameObject);
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, _player.transform.position);

        if (dist <= radius) 
            Collect();
    }
}
