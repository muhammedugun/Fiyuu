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
                // Oyun sahnesinde Singleton nesnesini bul veya oluþtur
                instance = FindObjectOfType<Infrastructure>();

                if (instance == null)
                {
                    // Eðer sahnede bulunamazsa yeni bir GameObject oluþtur
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
            Debug.LogWarning("sahne yüklendi: " + SceneManager.GetSceneByBuildIndex(level).name);

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
        
        // Sahneyi yükle ve yükleme iþleminin tamamlanmasýný bekle
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // progress 0 ile 0.9 arasýnda deðiþir, bu yüzden normalize ediyoruz.
            Debug.Log("Loading progress: " + (progress * 100f).ToString("F2") + "%");
            Debug.Log("progress: " + progress);
            loadingSlider.value = progress;
            await Task.Yield();
        }

        // Yükleme tamamlandýktan sonra buraya gelecek kod
        Debug.Log("Scene loaded: " + SceneManager.GetActiveScene().name);
    }


}
