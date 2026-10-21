using UnityEngine;

public class WinState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        if (UIManager.I != null) UIManager.I.ShowWinScreen();

        if (AudioManager.I != null) AudioManager.I.PlayWinStinger();
    }
    public void Tick() { }
    public void Exit() { }
}