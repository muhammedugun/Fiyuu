using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InLevelManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> winFireworks;
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
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5)));
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5))+1);
        EventBus.Publish(EventType.LevelEnd);
        foreach (var item in winFireworks)
        {
            item.Play();
        }
        Debug.LogWarning("Oyun Bitti");
        Invoke(nameof(StopGame), 2f);

    }

    private void StopGame()
    {
        Time.timeScale = 0f;
    }

}
