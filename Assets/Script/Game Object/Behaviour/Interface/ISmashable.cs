using UnityEngine;
/// <summary>
/// Parçalanabilir objeler
/// </summary>
public interface ISmashable : IDamageable
{
    /// <summary>
    /// Parçalamayý gerçekleþtirir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    void Smash(Collision collision);

    /// <summary>
    /// Parçalamanýn gerçekleþebilirliðini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    bool CheckSmash();
}
