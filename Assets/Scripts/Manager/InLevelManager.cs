// Refactor 23.08.24
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Seviyeler içinde genel durumlarý yönetmekten sorumludur
/// </summary>
public class InLevelManager : MonoBehaviour
{
    /// <summary>
    /// Tüm objeler durdu mu?
    /// </summary>
    public static bool isAllObjectsStopped;

    [SerializeField] private List<ParticleSystem> _winFireworks;

    private Rigidbody[] _allGameObjects;
    /// <summary>
    /// Objelerin hýzý kontrol edilsin mi?
    /// </summary>
    private bool _isCheckSpeedOfObjects;
    private bool _isAssignObjects, _isSetCheckSpeedOfObjects;
    private const string _muteKey = "isMute";

    private void Start()
    {
        isAllObjectsStopped = false;
        CheckAudioVolume();
    }

    private void Update()
    {
        if(CheckSpeedOfObjects())
        {
            Debug.Log("Tüm objeler durdu");
            EventBus.Publish(EventType.AllObjectsStopped);
        }
            
    }

    private void OnEnable()
    {
        ControllerManager.controller.InLevel.Attack.started += CheckFirstClick;

        EventBus.Subscribe(EventType.AllEnemiesDead, LevelSuccesful);

        EventBus.Subscribe(EventType.OutOfAmmo, AssignObjects);
        EventBus.Subscribe(EventType.OutOfAmmo, InvokeSetCheckSpeedOfObjects);

        EventBus.Subscribe(EventType.AllEnemiesDead, AssignObjects);
        EventBus.Subscribe(EventType.AllEnemiesDead, InvokeSetCheckSpeedOfObjects);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.AllEnemiesDead, LevelSuccesful);
        EventBus.Unsubscribe(EventType.OutOfAmmo, AssignObjects);
        EventBus.Unsubscribe(EventType.OutOfAmmo, InvokeSetCheckSpeedOfObjects);

        EventBus.Unsubscribe(EventType.AllEnemiesDead, AssignObjects);
        EventBus.Unsubscribe(EventType.AllEnemiesDead, InvokeSetCheckSpeedOfObjects);
    }

    /// <summary>
    /// Oyun sesini kontrol eder
    /// </summary>
    private void CheckAudioVolume()
    {
        bool isMuted = PlayerPrefs.GetInt(_muteKey) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    /// <summary>
    /// Objelerin hýzýný kontrol et
    /// </summary>
    /// <returns>Tüm objeler durduysa true döndürür</returns>
    private bool CheckSpeedOfObjects()
    {
        if (_isCheckSpeedOfObjects && !isAllObjectsStopped)
        {
            bool isAllObjectsStopped = true;

            foreach (var obj in _allGameObjects)
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

            InLevelManager.isAllObjectsStopped = isAllObjectsStopped;
            return true;

        }
        else
            return false;
    }

    /// <summary>
    /// Leveldeki tüm ammo, enemy ve building objelerini allGameObjects listesine kaydeder.
    /// </summary>
    private void AssignObjects()
    {
        if(!_isAssignObjects)
        {
            _isAssignObjects = true;
            _allGameObjects = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        }
        
    }

    void InvokeSetCheckSpeedOfObjects()
    {
        if(!_isSetCheckSpeedOfObjects)
        {
            _isSetCheckSpeedOfObjects = true;
            Invoke(nameof(SetCheckSpeedOfObjects), 1f);
        }
    }

    void SetCheckSpeedOfObjects()
    {
        _isCheckSpeedOfObjects = true;
    }

    /// <summary>
    /// Level baþlangýcýndaki ilk týklama gerçekleþtiyse bunu ilgili evente abone olanlara bildirir.
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

    /// <summary>
    /// Seviyenin baþarýyla geçildiði bilgisini kaydeder ve seviye tamamlanýnca olacak olaylarý gerçekleþtirir.
    /// </summary>
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
