using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// F�rlat�c� silahlar�(manc�n�klar�) temsil eder.
/// </summary>
public abstract class Launcher : MonoBehaviour
{
    [Tooltip("M�himmat�n rigidbody bile�eni")]
    protected Rigidbody _ammoRigidBody;
    [Tooltip("F�rlatma g�c�")]
    [SerializeField] protected float _launchPower;
    [SerializeField ]protected Animator _animator;
    /// <summary>
    /// M�himmat�n ba�lang��taki y pozisyonu
    /// </summary>
    protected float _ammoStartY;

    
    /// <summary>
    /// F�rlatma i�lemini ba�lat�r
    /// </summary>
    /// <param name="context"></param>
    protected abstract void Launch();

    /// <summary>
    /// F�rlatma animasyonunu tetikle
    /// </summary>
    protected virtual void LaunchAnimTrigger()
    {
         _animator.SetTrigger("launch");
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

}
