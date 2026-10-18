using UnityEngine;

public class PauseState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        if (UIManager.I != null) UIManager.I.ShowPauseMenu();
    }
    public void Tick() { }
    public void Exit() { }
}