using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotweenTest : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.DOFade(1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
