using UnityEngine;

public class WallLayer : HasHealth
{
    [SerializeField]
    private LayerTier _tier = LayerTier.Base;
    public LayerTier Tier => _tier;

    [ReadOnly]
    public int OveralLayerlLevel;

    private void OnEnable() => OnDeath += HandleWallBreak;
    private void OnDisable() => OnDeath -= HandleWallBreak;

    protected virtual void Start()
    {
        if (FortressLayerManager.I != null && _tier > FortressLayerManager.I.CurrentTier)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void HandleWallBreak() => gameObject.SetActive(false);
    public void Rebuild()
    {
        ResetHealth();
        gameObject.SetActive(true);
    }
}

public enum LayerTier
{
    Base = 1,
    InnerWall = 2,
    MiddleLayer = 3,
    OuterWall = 4
}