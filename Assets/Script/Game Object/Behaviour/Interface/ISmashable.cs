using UnityEngine;
/// <summary>
/// Parçalanabilir objeler
/// </summary>
public interface ISmashable : IDamageable
{
    /// <summary>
    /// Parçalamayı gerçekleştirir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneği</param>
    void Smash(Collision collision);

    /// <summary>
    /// Parçalamanın gerçekleşebilirliğini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    bool CheckSmash();
}
