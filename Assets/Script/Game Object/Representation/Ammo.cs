using UnityEngine;

/// <summary>
/// Mühimmat ile ilgili
/// </summary>
public class Ammo : MonoBehaviour
{
    [Tooltip("Mühimmatýn maddesi")]
    [SerializeField] internal AmmoMatter matter;
}

/// <summary>
/// Mühimmatýn maddesi
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