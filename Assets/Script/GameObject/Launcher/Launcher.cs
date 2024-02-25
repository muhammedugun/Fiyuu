using System;
using UnityEngine;
using Zenject;


/// <summary>
/// F�rlat�c� silahlar�(manc�n�klar�) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    public static event Action OnLaunched;
    [SerializeField] protected Transform _ammoSpawnPosition;

    [Tooltip("M�himmat�n rigidbody bile�eni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("F�rlatma g�c�")]
    [SerializeField] protected float _launchPower;
    [SerializeField] protected Animator _animator;
    
    protected GameObject _ammo;
    [SerializeField] protected Transform _ammoParent;
    /// <summary>
    /// M�himmat�n ba�lang��taki y pozisyonu
    /// </summary>
    protected float _ammoStartY;
    protected float _currentLaunchPower;

    [Inject] private AmmoSelectionUI _ammoSelection;
    private void Awake()
    {
        _currentLaunchPower = _launchPower;
    }

    /// <summary>
    /// F�rlatma g�c�n� a��rl��a g�re g�ncelle
    /// </summary>
    internal protected void UpdateLaunchPower()
    {

        _currentLaunchPower = _launchPower * _ammoRigidBody.mass;
    }

    /// <summary>
    /// F�rlatma g�c�n� a��rl��a g�re g�ncelle
    /// </summary>
    internal virtual void UpdateLaunchPowerInvoke()
    {

        Invoke(nameof(UpdateLaunchPower), 0.1f);
    }


    /// <summary>
    /// Animasyon state de�i�imi i�in kontrol ger�ekle�tir. 
    /// </summary>
    /// <returns>State de�i�ebilecek durumdaysa true, de�ilse false d�nd�r�r</returns>
    protected virtual bool CheckChangeAnimState()
    {

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            return true;
        else return false;
    }

    protected void OnLaunchedInvoke()
    {
        OnLaunched?.Invoke();
    }
}
