using UnityEngine;
using System.Collections;

public class GoldDrop : HasPool<GoldDrop>
{
    [SerializeField] 
    private float _lifetime = 5f;
    [SerializeField]
    [ReadOnly] 
    private float _multiplier = 3f;
    [SerializeField]
    [ReadOnly] 
    private int _gold;

    private Coroutine _lifetimeCoroutine;

    private void OnEnable() => _lifetimeCoroutine = StartCoroutine(LifetimeCountdown());
    private void OnDisable() => StopCoroutine(_lifetimeCoroutine);

    private IEnumerator LifetimeCountdown()
    {
        yield return new WaitForSeconds(_lifetime);
        Release(this);
    }

    public GoldDrop SetGoldValue(int value)
    {
        _gold = (int)(value * Random.Range(1f, _multiplier));
        return this;
    }

    public void SpawnPopup(CoinPopup coinPopupPrefab, Canvas worldCanvas, Vector3 spawnScreenPos) =>
        GameplayFactory.I.GetEntity(
            coinPopupPrefab,
            worldCanvas.transform,
            spawnScreenPos)
        .SetSpawn(spawnScreenPos);
}
