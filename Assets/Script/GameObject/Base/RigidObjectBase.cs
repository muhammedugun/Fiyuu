// Refactor 12.03.24
using UnityEngine;

/// <summary>
/// Hacmi ve aðýrlýðý olan katý cisimler için base class.
/// <para>Not: Fizikten etkilenecek olan nesneler için gereklidir.</para>
/// </summary>
public abstract class RigidObjectBase : MonoBehaviour
{
    public Renderer meshRenderer;

    protected Rigidbody _rigidbody;
    /// <summary>
    /// Aðýrlýk atamasý yapýlýrken eklenecek çarpan.
    /// </summary>
    protected float _massMultiplier;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kapladýðý alanýn boyutu.
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
    /// Objenin aðýrlýðýný atar
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="volumeSize"></param>
    /// <param name="multiplier">Aðýrlýk çarpaný</param>
    protected void AssignMass(Rigidbody rb, float volumeSize, float multiplier)
    {
        if (volumeSize <= 0)
            Debug.LogError("Mühimmatýn hacmi atanmamýþ ya da hacmi sýfýr");
        else
            rb.mass = multiplier * (volumeSize);
    }
}
