using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InLevelManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _winFireworks;

    Rigidbody []allGameObjects;
    /// <summary>
    /// Objelerin h�z� kontrol edilsin mi?
    /// </summary>
    private bool _isCheckSpeedOfObjects;
    /// <summary>
    /// T�m objeler durdu mu?
    /// </summary>
    private bool _isAllObjectsStopped;
    private const string muteKey = "isMute";

    private void Start()
    {
        CheckAudioVolume();
    }

    private void Update()
    {
        if(CheckSpeedOfObjects())
            EventBus.Publish(EventType.AllObjectsStopped);
    }

    private void OnEnable()
    {
        ControllerManager.controller.InLevel.Attack.started += CheckFirstClick;

        EventBus.Subscribe(EventType.AllEnemiesDead, LevelSuccesful);

        EventBus.Subscribe(EventType.OutOfAmmo, AssignObjects);
        EventBus.Subscribe(EventType.OutOfAmmo, InvokeSetCheckSpeedOfObjects);
        
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.AllEnemiesDead, LevelSuccesful);
        EventBus.Unsubscribe(EventType.OutOfAmmo, AssignObjects);
        EventBus.Unsubscribe(EventType.OutOfAmmo, InvokeSetCheckSpeedOfObjects);
    }

    private void CheckAudioVolume()
    {
        bool isMuted = PlayerPrefs.GetInt(muteKey) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    /// <summary>
    /// Objelerin h�z�n� kontrol et
    /// </summary>
    /// <returns>T�m objeler durduysa true d�nd�r�r</returns>
    private bool CheckSpeedOfObjects()
    {
        if (_isCheckSpeedOfObjects && !_isAllObjectsStopped)
        {
            bool isAllObjectsStopped = true;

            foreach (var obj in allGameObjects)
            {
                if (obj != null)
                {
                    float speed = obj.velocity.magnitude;
                    if (speed > 1f)
                    {
                        return false;
                    }
                }
            }

            _isAllObjectsStopped = isAllObjectsStopped;
            return true;

        }
        else
            return false;
    }

    /// <summary>
    /// Leveldeki t�m ammo, enemy ve building objelerini allGameObjects listesine kaydeder.
    /// </summary>
    private void AssignObjects()
    {
        allGameObjects = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
    }

    void InvokeSetCheckSpeedOfObjects()
    {
        Invoke(nameof(SetCheckSpeedOfObjects), 1f);
    }

    void SetCheckSpeedOfObjects()
    {
        _isCheckSpeedOfObjects = true;
    }

    /// <summary>
    /// Level ba�lang�c�ndaki ilk t�klama ger�ekle�tiyse bunu ilgili evente abone olanlara bildirir.
    /// </summary>
    /// <param name="context"></param>
    void CheckFirstClick(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "Level1" && PlayerPrefs.GetInt("Level1Played") == 0)
        {
            PlayerPrefs.SetInt("Level1Played", 1);
        }
        else
        {
            EventBus.Publish(EventType.FirstClickInLevel);
            ControllerManager.controller.InLevel.Attack.started -= CheckFirstClick;
        }
    }

    
    void LevelSuccesful()
    {
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5)));
        ChaptersManager.CompleteLevel(int.Parse(SceneManager.GetActiveScene().name.Substring(5))+1);
        foreach (var item in _winFireworks)
        {
            item.Play();
        }

    }


}
