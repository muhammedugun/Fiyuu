using UnityEngine;

/// <summary>
/// M�himmat ile ilgili
/// </summary>
public class Ammo : MonoBehaviour
{
    [Tooltip("M�himmat�n maddesi")]
    [SerializeField] internal AmmoMatter matter;
}

/// <summary>
/// M�himmat�n maddesi
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