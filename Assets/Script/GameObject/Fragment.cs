using UnityEngine;

public class Fragment : MonoBehaviour
{
    public GameObject particleEffect;
    /// <summary>
    /// Kaç saniye sonra yok olacaðý
    /// </summary>
    [SerializeField] private float deactivedDuration = 1f;

    internal bool isUsingParticle = false;
    void Start()
    {
        
        Invoke(nameof(SetDeactive), deactivedDuration);
    }

    private void SetDeactive()
    {
        
        if (isUsingParticle)
        {
            float volumeSize = 1f;
            AssignVolume(GetComponent<MeshRenderer>(), ref volumeSize);
            var particle = Instantiate(particleEffect);
            particle.transform.SetPositionAndRotation(transform.position, transform.rotation);
            float ortalama = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
            particle.transform.localScale = Vector3.one * ortalama;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    private void AssignVolume(Renderer renderer, ref float volumeSize)
    {
        Bounds bounds = renderer.bounds;
        volumeSize = bounds.size.x * bounds.size.y * bounds.size.z;
    }

}
