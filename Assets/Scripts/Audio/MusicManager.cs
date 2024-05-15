using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isStop;

    private static MusicManager instance;
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(MusicManager).Name);
                    instance = singletonObject.AddComponent<MusicManager>();
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
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        string name = SceneManager.GetSceneByBuildIndex(level).name;
        if (name == "Chapters" || name == "Loading" || name == "MainMenu" || name == "DemoEnd")
        {
            if (isStop)
            {
                audioSource.Play();
                isStop = false;
            }
                
            else if (!audioSource.isPlaying)
                audioSource.UnPause();
            
        }
        else
        {
            audioSource.Stop();
            isStop = true;
        }
    }

}
