using System.Collections;
using UnityEngine;

public class ParticleObject : HasPool<ParticleObject>
{
    [SerializeField]
    private GameObject _particleToPlay;
    [SerializeField]
    private float _lifetime = 1f;

    private void OnEnable() => StartCoroutine(PlayParticle());
    
    private IEnumerator PlayParticle() 
    {
        yield return new WaitForSeconds(_lifetime);
        gameObject.SetActive(false);
        _pool.Release(this);
    }
}
