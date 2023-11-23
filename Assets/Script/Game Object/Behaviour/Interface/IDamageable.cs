using UnityEngine;

public interface IDamageable
{

    /// <summary>
    /// Objenin dayan�kl�l���
    /// </summary>
    float Durability { get; set;}

    /// <summary>
    /// Objenin dayan�kl�k de�erini azalt�r
    /// </summary>
    /// <param name="collision">�arp���lan obje</param>
    void DoDamage(Collision collision);
}
