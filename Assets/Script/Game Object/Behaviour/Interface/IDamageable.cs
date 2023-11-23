using UnityEngine;

public interface IDamageable
{

    /// <summary>
    /// Objenin dayanýklýlýðý
    /// </summary>
    float Durability { get; set;}

    /// <summary>
    /// Objenin dayanýklýk deðerini azaltýr
    /// </summary>
    /// <param name="collision">Çarpýþýlan obje</param>
    void DoDamage(Collision collision);
}
