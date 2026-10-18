using UnityEngine;

public class WinState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        if (UIManager.I != null) UIManager.I.ShowWinScreen();
    }
    public void Tick() { }
    public void Exit() { }
}