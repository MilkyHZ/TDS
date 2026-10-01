using UnityEngine;

public class TurretBulletBehaviour : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 15f;
    public float Speed 
    {
        get { return _speed; }
        set { _speed = value; }
    }
    [SerializeField]
    private float _killRadius = 0.8f;
    public float KillRadius
    {
        get { return _killRadius; }
        set { _killRadius = value; }
    }
    private float _lifeTime = 4f;

    private void Start() => Destroy(gameObject, _lifeTime);

    private void Update()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

        CreatureBehaviour hitCreature = CheckNearestEnemy();
        if (hitCreature == null) return;
        
        KillCreature(hitCreature);
    }

    private CreatureBehaviour CheckNearestEnemy() 
    {
        Vector3 bulletFlat = new(transform.position.x, 0, transform.position.z);

        CreatureBehaviour[] allCreatures = Object.FindObjectsByType<CreatureBehaviour>(FindObjectsSortMode.None);

        foreach (CreatureBehaviour creature in allCreatures) 
        {
            Vector3 targetFlat = new(creature.transform.position.x, 0, creature.transform.position.z);
            float dist = Vector3.Distance(bulletFlat, targetFlat);

            if (dist <= _killRadius)
            {
                return creature;
            }
        }
        return null;
    }

    private void KillCreature(CreatureBehaviour hit) 
    {
        hit.Die();
        if (_lifeTime > 0)
            Destroy(gameObject);
    }
}
