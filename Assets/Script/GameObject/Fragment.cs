// Refactor 12.03.24
using UnityEngine;

/// <summary>
/// Smash (OpenFracture) sistemindeki bir par�a
/// </summary>
public class Fragment : MonoBehaviour
{
    /// <summary>
    /// Obje olu�tuktan ka� saniye sonra yok olsun?
    /// </summary>
    [SerializeField] private float deactivedDuration = 5f;

    /// <summary>
    /// Obje yok olurken �retilecek olan partikul efekti
    /// </summary>
    internal GameObject particleEffect;
    internal bool isUsingParticle = false;
    void Start()
    {
        float duration = Random.Range(deactivedDuration - 3f, deactivedDuration);
        Invoke(nameof(SetDeactive), duration);
    }

    private void SetDeactive()
    {
        
        if (isUsingParticle)
        {
            var particle = Instantiate(particleEffect);
            particle.transform.SetPositionAndRotation(transform.position, transform.rotation);
            float middle = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
            particle.transform.localScale = Vector3.one * middle;
        }
        gameObject.SetActive(false);
    }

}
