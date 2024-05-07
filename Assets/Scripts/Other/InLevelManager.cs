using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InLevelManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> winFireworks;
    [SerializeField] private InputRange _inputRange;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private bool isTutorialScene;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        EventBus.Subscribe(EventType.OutOfAmmo, GameOver);
        EventBus.Subscribe(EventType.AllEnemiesDead, GameOver);
        if(isTutorialScene)
            _inputRange.started += SetTutorial;
    }

    private void UnSubscribe()
    {
        EventBus.Unsubscribe(EventType.OutOfAmmo, GameOver);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, GameOver);
    }

    private void SetTutorial()
    {
        tutorial.SetActive(false);
        _inputRange.started -= SetTutorial;
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
