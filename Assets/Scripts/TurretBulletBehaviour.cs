using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private float _lifeTime = 4f;

    private Transform _target;

    private void Start()
    {
        GameObject target = GameObject.FindWithTag("Player");
        if (target != null) _target = target.transform;

        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime), Space.Self);

        if (_target == null) return;

        Vector3 bulletFlat = new (transform.position.x, 0, transform.position.z);
        Vector3 targetFlat = new (_target.position.x, 0, _target.position.z);

        float distToPlayer = Vector3.Distance(bulletFlat, targetFlat);
        if (distToPlayer <= _killRadius)
        {
            KillPlayer();
        }
    }
    private void KillPlayer() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
