using UnityEngine;

public class TitleState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;

        if (UIManager.I != null) UIManager.I.ShowTitleScreen();
        if (MovingFloor.I != null) MovingFloor.I.MoveToTitlePosition();
        if (AudioManager.I != null) AudioManager.I.PlayMenuMusic();
    }
    public void Tick() { }
    public void Exit() { }
}