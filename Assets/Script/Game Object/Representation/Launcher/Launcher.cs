using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Fýrlatýcý silahlarý(mancýnýklarý) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    [Tooltip("Mühimmatýn rigidbody bileþeni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("Fýrlatma gücü")]
    [SerializeField] protected float _launchPower;
    [SerializeField ]protected Animator _animator;
    /// <summary>
    /// Mühimmatýn baþlangýçtaki y pozisyonu
    /// </summary>
    protected float _ammoStartY;

    
    /// <summary>
    /// Fýrlatma iþlemini baþlatýr
    /// </summary>
    /// <param name="context"></param>
    protected abstract void Launch();

    /// <summary>
    /// Fýrlatma animasyonunu tetikle
    /// </summary>
    protected virtual void LaunchAnimTrigger()
    {
         _animator.SetTrigger("launch");
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

}
