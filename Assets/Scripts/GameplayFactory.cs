    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;

    public class GameplayFactory : MonoBehaviour
    {
        public static GameplayFactory I;

        private Dictionary<Component, object> _pools = new();

        private readonly bool _collectionCheck = true;
        [SerializeField]
        private int _defaultCapacity = 20;
        [SerializeField]
        private int _maxCapacity = 1000;
        private void Awake()
        {
            I = this;
        }

        private IObjectPool<T> CreateEntityPool<T>(T prefab) where T : Component
        {
            IObjectPool<T> pool = null;

            return pool = new ObjectPool<T>(
                () => {
                    T entity = CreateEntity(prefab);

                    if (entity.TryGetComponent<HasPool<T>>(out var poolComponent))
                    {
                        poolComponent.SetPool(pool);
                    }

                    return entity;
                },
                OnGet,
                OnRelease,
                OnDestroyEntity,
                _collectionCheck,
                _defaultCapacity,
                _maxCapacity
            );
        }

        //3D entities
        public T GetEntity<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
        {
            IObjectPool<T> targetPool = CreateOrGetEntityPool(prefab);
            T entity = targetPool.Get();
            entity.transform.SetPositionAndRotation( pos, rot );
        
            return entity;
        }

        //UI entities
        public T GetEntity<T>(T prefab, Transform parent, Vector2 pos) where T : Component
        {
            IObjectPool<T> targetPool = CreateOrGetEntityPool(prefab);
            T entity = targetPool.Get();
            entity.transform.SetParent(parent, false);

            if (entity.TryGetComponent<RectTransform>(out var rect))
            {
                rect.position = pos;
            }

            return entity;
        }

        public IObjectPool<T> CreateOrGetEntityPool<T>(T prefab) where T : Component
        {
            IObjectPool<T> targetPool;

            if (_pools.TryGetValue(prefab, out object poolObj))
            {
                targetPool = (IObjectPool<T>)poolObj;
            }
            else
            {
                targetPool = CreateEntityPool(prefab);
                _pools.Add(prefab, targetPool);
            }

            return targetPool;
        }

        private T CreateEntity<T>(T prefab) where T : Component => Instantiate(prefab, transform);
        private void OnGet<T>(T entity) where T: Component => entity.gameObject.SetActive(true);
        private void OnRelease<T>(T entity) where T : Component => entity.gameObject.SetActive(false);
        private void OnDestroyEntity<T>(T entity)where T : Component => Destroy(entity.gameObject);
    }