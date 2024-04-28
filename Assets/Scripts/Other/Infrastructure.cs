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
                // Oyun sahnesinde Singleton nesnesini bul veya olu�tur
                instance = FindObjectOfType<Infrastructure>();

                if (instance == null)
                {
                    // E�er sahnede bulunamazsa yeni bir GameObject olu�tur
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
            Debug.LogWarning("sahne y�klendi: " + SceneManager.GetSceneByBuildIndex(level).name);

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
        
        // Sahneyi y�kle ve y�kleme i�leminin tamamlanmas�n� bekle
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // progress 0 ile 0.9 aras�nda de�i�ir, bu y�zden normalize ediyoruz.
            Debug.Log("Loading progress: " + (progress * 100f).ToString("F2") + "%");
            Debug.Log("progress: " + progress);
            loadingSlider.value = progress;
            await Task.Yield();
        }

        // Y�kleme tamamland�ktan sonra buraya gelecek kod
        Debug.Log("Scene loaded: " + SceneManager.GetActiveScene().name);
    }


}
