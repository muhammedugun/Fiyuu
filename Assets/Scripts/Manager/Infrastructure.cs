// Refactor 23.08.24
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Oyunun ilk açýlýþý için gereklilikleri yönetmekten sorumlu
/// </summary>
public class Infrastructure : MonoBehaviour
{
    private static Slider _loadingSlider;
    private static string _currentSceneName;
    private static Infrastructure _instance;

    public static Infrastructure Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Infrastructure>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Infrastructure).Name);
                    _instance = singletonObject.AddComponent<Infrastructure>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _currentSceneName = null;
        }
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync'i kapatýr
        Application.targetFrameRate = 60; // Ýsteðe baðlý: FPS’i sýnýrlamak için

        _loadingSlider = FindObjectOfType<Slider>();
        if (_currentSceneName==null)
        {
            LoadSceneAsync("MainMenu");
        } 
    }

    private void OnLevelWasLoaded(int level)
    {
        if (_instance != null && _instance == this && SceneManager.GetSceneByBuildIndex(level).name == "Loading")
        {

            _loadingSlider = FindObjectOfType<Slider>();

            if(_currentSceneName != null && _loadingSlider!=null)
                LoadSceneAsync(_currentSceneName);
        }
            
    }

    public static void LoadScene(string sceneName)
    {
        _currentSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// Parametre olarak gönderilen sahneyi asenkron olarak yükler
    /// </summary>
    /// <param name="sceneName"></param>
    public static async void LoadSceneAsync(string sceneName)
    { 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            _loadingSlider.value = progress;
            await Task.Yield();
        }
    }

}
