using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurrretPlacementmanager : MonoBehaviour
{
    public static TurrretPlacementmanager I;

    [SerializeField]
    [Range(0,5)]
    private int _turretCount = 1;

    [SerializeField]
    private TurretPlacementPoint _placementPointPrefab;

    private Dictionary<BaseTurret, LayerTier> _turretLayer = new();

    private void Awake() => I = this;

    private void CreateTowerPlacement() 
    {
        
    }

    private void RemoveAllTurrets(LayerTier tier) 
    {
        var entriesToRemove = _turretLayer.Where(tl => tl.Value == tier).ToList();

        foreach (var entry in entriesToRemove) 
        {
            _turretLayer.Remove(entry.Key);
        }
    }
}