using UnityEngine;

public class Infrastructure : MonoBehaviour
{
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // TODO: ansync olarak ana menuye yönlendir
    }

  
}
