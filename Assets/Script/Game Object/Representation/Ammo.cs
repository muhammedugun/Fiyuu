using UnityEngine;

public class Ammo : MonoBehaviour
{
    [Tooltip("M�himmat�n z�rh�")]
    [SerializeField] internal AmmoMatter armor;

    

}

/// <summary>
/// Madde
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