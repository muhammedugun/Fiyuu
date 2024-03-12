// Refactor 12.03.24
using UnityEngine;

/// <summary>
/// Hacmi ve a��rl��� olan kat� cisimler i�in base class.
/// <para>Not: Fizikten etkilenecek olan nesneler i�in gereklidir.</para>
/// </summary>
public abstract class RigidObjectBase : MonoBehaviour
{
    public Renderer meshRenderer;

    protected Rigidbody _rigidbody;
    /// <summary>
    /// A��rl�k atamas� yap�l�rken eklenecek �arpan.
    /// </summary>
    protected float _massMultiplier;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kaplad��� alan�n boyutu.
    /// </summary>
    protected float _volumeSize;


    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        AssignVolume(meshRenderer, ref _volumeSize);
        AssignMass(_rigidbody, _volumeSize, _massMultiplier);
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    protected void AssignVolume(Renderer renderer, ref float volumeSize)
    {
        Bounds bounds = renderer.bounds;
        volumeSize = bounds.size.x * bounds.size.y * bounds.size.z;
    }

    /// <summary>
    /// Objenin a��rl���n� atar
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="volumeSize"></param>
    /// <param name="multiplier">A��rl�k �arpan�</param>
    protected void AssignMass(Rigidbody rb, float volumeSize, float multiplier)
    {
        if (volumeSize <= 0)
            Debug.LogError("M�himmat�n hacmi atanmam�� ya da hacmi s�f�r");
        else
            rb.mass = multiplier * (volumeSize);
    }
}
