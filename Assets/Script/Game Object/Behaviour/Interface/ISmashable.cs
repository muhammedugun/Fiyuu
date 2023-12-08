using UnityEngine;
/// <summary>
/// Par�alanabilir objeler
/// </summary>
public interface ISmashable : IDamageable
{
    /// <summary>
    /// Par�alamay� ger�ekle�tirir
    /// </summary>
    /// <param name="smashableObject">Objenin par�alanabilir halinin �rne�i</param>
    void Smash(GameObject smashableObject);

    /// <summary>
    /// Par�alaman�n ger�ekle�ebilirli�ini kontrol eder
    /// </summary>
    /// <param name="collision"></param>
    bool CheckSmash();
}
