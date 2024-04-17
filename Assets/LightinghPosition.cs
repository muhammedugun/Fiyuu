using UnityEngine;

public class LightinghPosition : MonoBehaviour
{

    public void RandomizePosition()
    {
        transform.position = new Vector3(Random.Range(-26f, 0f), Random.Range(11.5f, 13f));
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-60f, 0f));
    }
}
