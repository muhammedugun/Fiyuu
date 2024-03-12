using TMPro;
using UnityEngine;

public class RectTransformTest : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, gameObject.transform.position);
        Debug.Log("screenpoint:" + screenPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(text.rectTransform, screenPoint, null, out Vector2 localPoint);
        Debug.Log("localpoint: " + localPoint);
    }

}