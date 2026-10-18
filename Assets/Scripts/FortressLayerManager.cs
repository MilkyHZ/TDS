using UnityEngine;
using System;
using System.Collections.Generic;

public class FortressLayerManager : MonoBehaviour
{
    public static FortressLayerManager I;

    [SerializeField]
    private float _basePlayerRadius = 3f;
    public float BasePlayerRadius => _basePlayerRadius;

    private float _currentRadius;
    public float CurrentRadius => _currentRadius;

    private Dictionary<LayerTier, WallLayer> _layers = new();

    private LayerTier _currentTier;
    public LayerTier CurrentTier
    {
        get => _currentTier;
        set
        {
            if (_currentTier != value)
            {
                _currentTier = value;
                UpdateRadius();
                OnTierChanged?.Invoke();
            }
        }
    }

    public event Action OnTierChanged;

    private void Awake() => I = this;

    private void Start()
    {
        Init();
        SetActiveTier(LayerTier.Base); 
    }

    private void Init() 
    {
        _layers.Clear();
        var walls = FindObjectsByType<WallLayer>(FindObjectsSortMode.None);
        foreach (var wall in walls) 
        {
            if (_layers.ContainsKey(wall.Tier)) continue;
            _layers.Add(wall.Tier, wall);

            wall.OnDeath += () => HandleWallDestroy(wall.Tier);
        }
    }

    public WallLayer GetActiveWall()
    {
        if (_layers.TryGetValue(_currentTier, out var wall))
            return wall;

        return null;
    }

    private void HandleWallDestroy(LayerTier tier) 
    {
        if (tier == _currentTier)
        {
            LayerTier nextTier = _currentTier switch
            {
                LayerTier.OuterWall => LayerTier.MiddleLayer,
                LayerTier.MiddleLayer => LayerTier.InnerWall,
                _ => LayerTier.Base
            };

            SetActiveTier(nextTier);
        }
    }

    private void SetActiveTier(LayerTier tier) => _currentTier = tier;

    private void UpdateRadius() => GetRadiusForTier(_currentTier, _basePlayerRadius);

    public static float GetRadiusForTier(LayerTier tier, float baseRadius)
    {
        float multiplier = tier switch
        {
            LayerTier.InnerWall => (int)LayerTier.InnerWall,
            LayerTier.MiddleLayer => (int)LayerTier.MiddleLayer,
            LayerTier.OuterWall => (int)LayerTier.OuterWall,
            _ => 1
        };
        return multiplier * baseRadius;
    }
}