using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        ControllerManager.Subscribe(ManagedFonk);
        EventBus.Subscribe(EventType.AllEnemiesDead, LevelSuccesful);
        if(isTutorialScene)
            _inputRange.started += SetTutorial;
    }

    private void UnSubscribe()
    {
        EventBus.Unsubscribe(EventType.AllEnemiesDead, LevelSuccesful);
    }

    void ManagedFonk(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "Level1" && PlayerPrefs.GetInt("Level1Played") == 0)
        {
            PlayerPrefs.SetInt("Level1Played", 1);
        }
        else
        {
            EventBus.Publish(EventType.Clicked);
        }

        EventBus.Clear(EventType.Clicked);
    }

    private void SetTutorial()
    {
        tutorial.SetActive(false);
        _inputRange.started -= SetTutorial;
    }

    void LevelSuccesful()
    {
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5)));
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5))+1);
        foreach (var item in winFireworks)
        {
            item.Play();
        }

    }


}
