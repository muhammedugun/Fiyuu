using UnityEngine;

/// <summary>
/// M�himmat ile ilgili
/// </summary>
public class Ammo : MonoBehaviour
{
    [Tooltip("M�himmat�n maddesi")]
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
    /// Objenin a��rl���n� atar
    /// </summary>
    private void AssignMass(Rigidbody rb, float volumeSize)
    {
        if (volumeSize <= 0)
        {
            Debug.LogError("M�himmat�n hacmi atanmam�� ya da hacmi s�f�r");
        }
        else
        {
            rb.mass = power[(int)matter-1] * (volumeSize);
        }

    }

}

/// <summary>
/// M�himmat�n maddesi
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