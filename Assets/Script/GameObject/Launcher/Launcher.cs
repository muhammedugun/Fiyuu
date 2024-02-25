using System;
using UnityEngine;
using Zenject;


/// <summary>
/// Fýrlatýcý silahlarý(mancýnýklarý) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    public static event Action OnLaunched;
    [SerializeField] protected Transform _ammoSpawnPosition;

    [Tooltip("Mühimmatýn rigidbody bileþeni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("Fýrlatma gücü")]
    [SerializeField] protected float _launchPower;
    [SerializeField] protected Animator _animator;
    
    protected GameObject _ammo;
    [SerializeField] protected Transform _ammoParent;
    /// <summary>
    /// Mühimmatýn baþlangýçtaki y pozisyonu
    /// </summary>
    protected float _ammoStartY;
    protected float _currentLaunchPower;

    [Inject] private AmmoSelectionUI _ammoSelection;
    private void Awake()
    {
        _currentLaunchPower = _launchPower;
    }

    /// <summary>
    /// Fýrlatma gücünü aðýrlýða göre güncelle
    /// </summary>
    internal protected void UpdateLaunchPower()
    {

        _currentLaunchPower = _launchPower * _ammoRigidBody.mass;
    }

    /// <summary>
    /// Fýrlatma gücünü aðýrlýða göre güncelle
    /// </summary>
    internal virtual void UpdateLaunchPowerInvoke()
    {

        Invoke(nameof(UpdateLaunchPower), 0.1f);
    }


    /// <summary>
    /// Animasyon state deðiþimi için kontrol gerçekleþtir. 
    /// </summary>
    /// <returns>State deðiþebilecek durumdaysa true, deðilse false döndürür</returns>
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
