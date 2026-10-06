using UnityEngine;

public class HomingMissileBehaviour : MonoBehaviour
{
    private Transform _target;
    public Transform Target 
    {
        get { return _target; }
        set { _target = value; }
    }
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _turnSpeed = 2f;
    [SerializeField]
    private float _lifeTime = 5f;
    [SerializeField]
    private float _range = 1f;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {
        var dist = Vector3.Distance(_target.transform.position, transform.position);
        if (dist <= _range) 
            Explode();

        Vector3 dir = (_target.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }

    private void Explode() 
    {
        PlaneHealthManager.I.TakeDamage(1);
        Destroy(gameObject);
    }
}