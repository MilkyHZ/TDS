using UnityEngine;

public class TitleState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        if (UIManager.I != null) UIManager.I.ShowTitleScreen();
    }
    public void Tick() { }
    public void Exit() { }
}