// Refactor 12.03.24
using UnityEngine;

/// <summary>
/// Hacmi ve aðýrlýðý olan katý cisimler için base class.
/// <para>Not: Fizikten etkilenecek olan nesneler için gereklidir.</para>
/// </summary>
public abstract class RigidObjectBase : MonoBehaviour
{
    public MeshFilter meshFilter;

    protected Rigidbody _rigidbody;
    /// <summary>
    /// Aðýrlýk atamasý yapýlýrken eklenecek çarpan.
    /// </summary>
    protected float _massMultiplier=1f;
    /// <summary>
    /// Objenin hacmi. Yani objenin uzayda kapladýðý alanýn boyutu.
    /// </summary>
    protected float _volumeSize=1f;


    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if(meshFilter!=null)
            AssignVolume(meshFilter.mesh, ref _volumeSize);

        AssignMass(_rigidbody, _volumeSize, _massMultiplier);
    }

    /// <summary>
    /// Objenin hacmini atar
    /// </summary>
    protected void AssignVolume(Mesh mesh, ref float volumeSize)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        volume *= transform.localScale.x * transform.localScale.y * transform.localScale.z;

        volumeSize = Mathf.Abs(volume);
    }

    /// <summary>
    /// Hacim hesabý için gerekli bir fonksiyon
    /// </summary>
    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
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
