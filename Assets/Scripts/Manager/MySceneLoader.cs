using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Infrastructure.LoadScene(sceneName);
    }

}
