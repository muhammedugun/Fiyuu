//Refactor 23.08.24
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oyun boyunca (sahnelerden sahnelere ge�i�te bile) oyun m�zi�inin oynat�lma durumundan sorumludur
/// </summary>
public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    private static MusicManager instance;
    private bool isStop;

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
        // Singleton sa�lan�yor
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

    /// <summary>
    /// Bir sahne y�klendi�i zaman, y�klenen sahneye g�re m�zi�i oynat�r ya da duraklat�r
    /// </summary>
    /// <param name="level"></param>
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
