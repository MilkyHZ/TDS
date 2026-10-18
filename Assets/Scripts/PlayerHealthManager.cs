using UnityEngine;

public class PlayerHealthManager : WallLayer
{
    public static PlayerHealthManager I;

    private void Awake() => I = this;
    protected override void Start()
    {
        base.Start();
        ResetHealth();
    }

    protected override void HandleWallBreak() => GameControler.I.ChangeState(new GameOverState());

    private void Upgrade(int amount = 1) 
    {
        _maxHp += amount;
        CurrentHp += amount;
    }
}