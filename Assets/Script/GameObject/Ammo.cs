using UnityEngine;

/// <summary>
/// Mühimmat ile ilgili
/// </summary>
public class Ammo : MonoBehaviour
{
    [Tooltip("Mühimmatýn maddesi")]
    [SerializeField] internal AmmoMatter matter;
    internal float[] power = new float[8]{ 10f, 15f, 20f, 25f, 1f, 10f, 10f, 10f};
    float _volumeSize;

    private void Start()
    {
        AssignVolume(GetComponentInChildren<Renderer>(), ref _volumeSize);
        AssignMass(GetComponentInChildren<Rigidbody>(), _volumeSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy detected!");
                enemy.animator.SetTrigger("terrified");
            }
        }
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    private void AssignVolume(Renderer renderer, ref float volumeSize)
    {
        Bounds bounds = renderer.bounds;
        volumeSize = bounds.size.x * bounds.size.y * bounds.size.z;
    }

    /// <summary>
    /// Objenin aðýrlýðýný atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if (volumeSize <= 0)
        {
            Debug.LogError("Mühimmatýn hacmi atanmamýþ ya da hacmi sýfýr");
        }
        else
        {
            rb.mass = power[(int)matter-1] * (volumeSize);
        }

    }

}

/// <summary>
/// Mühimmatýn maddesi
/// </summary>
public enum AmmoMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel,
    Fire,
    Ice,
    Explosion,
    Electric
}