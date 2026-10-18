using UnityEngine;

public class GameOverState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        if (UIManager.I != null) UIManager.I.ShowGameOverScreen();
    }
    public void Tick() { }
    public void Exit() { }
}