using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Infrastructure : MonoBehaviour
{
    private static Slider loadingSlider;
    private static string currentSceneName;

    private static Infrastructure instance;
    public static Infrastructure Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Infrastructure>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Infrastructure).Name);
                    instance = singletonObject.AddComponent<Infrastructure>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentSceneName = null;
        }
    }


    void Start()
    {

        loadingSlider = FindObjectOfType<Slider>();
        if (currentSceneName==null)
        {
            LoadSceneAsync("MainMenu");
        } 
    }

    
    private void OnLevelWasLoaded(int level)
    {
        if (instance != null && instance == this && SceneManager.GetSceneByBuildIndex(level).name == "Loading")
        {

            loadingSlider = FindObjectOfType<Slider>();

            if(currentSceneName != null && loadingSlider!=null)
                LoadSceneAsync(currentSceneName);
        }
            
    }

    public static void LoadScene(string sceneName)
    {
        currentSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }


    public static async void LoadSceneAsync(string sceneName)
    {
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;
            await Task.Yield();
        }


    }


}
