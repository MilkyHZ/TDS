using UnityEngine;
using UnityEngine.Pool;

public class HasPool<T> : MonoBehaviour where T : Component
{
    protected IObjectPool<T> _pool;
    public void SetPool(IObjectPool<T> pool) => _pool = pool;

    public void Release(T entity) 
    {
        if (_pool != null) _pool.Release(entity);
        else gameObject.SetActive(false);
    }
}
