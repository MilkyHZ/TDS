using UnityEngine;

public class PlayingState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 1.0f;

        if (UIManager.I != null) UIManager.I.ShowGameplayUI();

        if (MovingFloor.I != null) MovingFloor.I.MoveToGameplayPosition();

        if (AudioManager.I != null) AudioManager.I.PlayGameplayMusic();
    }

    public void Tick()
    {
        if (PlayerHealthManager.I != null && PlayerHealthManager.I.CurrentHp <= 0)
        {
            GameControler.I.GameOver();
        }
    }

    public void Exit() { }
}