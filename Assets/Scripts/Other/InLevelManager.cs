using UnityEngine;


public class InLevelManager : MonoBehaviour
{

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.OutOfAmmo, GameOver);
        EventBus.Subscribe(EventType.AllEnemiesDead, GameOver);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.OutOfAmmo, GameOver);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, GameOver);
    }

    void GameOver()
    {
        EventBus.Publish(EventType.GameOver);
        Debug.LogWarning("Oyun Bitti");
        Invoke(nameof(StopGame), 0.1f);
    }

    private void StopGame()
    {
        Time.timeScale = 0f;
    }

}
