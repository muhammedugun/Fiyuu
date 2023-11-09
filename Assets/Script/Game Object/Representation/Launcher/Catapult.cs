using UnityEngine.InputSystem;
using Zenject;


public class Catapult : Launcher
{
    [Inject] FiringBar firingBar;
    private void Start()
    {
        PlayerController.action.InLevel.Attack.canceled += LaunchAnimTrigger;
    }

    protected override void Launch()
    {
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _launchPower * (1+(firingBar.currentFillAmount*100f)));
        _ammoRigidBody.transform.parent = null;
        
    }


    protected void LaunchAnimTrigger(InputAction.CallbackContext context)
    {
        if(CheckChangeAnimState())
        {
            _animator.SetTrigger("launch");
            _animator.speed = (firingBar.currentFillAmount * 6f) + 1;
        }
        
    }

}
