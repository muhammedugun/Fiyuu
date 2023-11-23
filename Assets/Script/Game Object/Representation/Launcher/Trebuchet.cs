using UnityEngine.InputSystem;
using UnityEngine;
public class Trebuchet : Launcher
{
    private void Awake()
    {
        PlayerController.action.InLevel.Attack.started += Launch;
    }

    private void Start()
    {
        _ammoStartY = _ammoRigidBody.transform.position.y;
    }

    protected override void Launch()
    {
        Launch();
    }

    /// <summary>
    /// Fýrlatma iþlemini baþlatýr
    /// </summary>
    /// <param name="context"></param>
    protected void Launch(InputAction.CallbackContext context)
    {
        if (CheckChangeAnimState()) { LaunchAnimTrigger(); }
        else if (_ammoRigidBody.isKinematic)
        {
            _animator.SetTrigger("leave");
            _ammoRigidBody.isKinematic = false;
            _ammoRigidBody.useGravity = true;
            float height = _ammoRigidBody.transform.position.y - _ammoStartY;
            Debug.Log(height);
            if(height<2)
            {
                height++;
            }
            
            _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _launchPower * height);
            _ammoRigidBody.transform.parent = null;
        }
    }
    
}
