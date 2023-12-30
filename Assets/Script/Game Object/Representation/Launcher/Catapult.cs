using Zenject;


public class Catapult : Launcher
{
    [Inject] FiringBar firingBar;
    [Inject] InputRangeTest inputRangeTest;


    private void Start()
    {
        AssignAmmoType();
        Initialize();
        //PlayerController.action.InLevel.Attack.canceled += LaunchAnimTrigger;
        inputRangeTest.exit += LaunchAnimTrigger;
        Invoke("SetLaunchPower", 0.1f); // birka� frame sonra �al��t�rmak ama�l�
    }

    /// <summary>
    /// F�rlat�rken yap�lmas� gerekirken
    /// </summary>
    protected override void Launch()
    {
        _ammoRigidBody.isKinematic = false;
        _ammoRigidBody.useGravity = true;
        _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _currentLaunchPower * (1+(firingBar.currentFillAmount*100f)));
        _ammoRigidBody.transform.parent = null;
        OnLaunchedInvoke();
    }
    

    protected void LaunchAnimTrigger()
    {
        if(CheckChangeAnimState())
        {
            _animator.SetTrigger("launch");
            _animator.speed = (firingBar.currentFillAmount * 6f) + 1;
        }
        
    }

}
