using UnityEngine;
using Zenject;

public class Trebuchet : Launcher
{
    [Inject] InputRangeTest inputRangeTest;
    private void Awake()
    {
        //PlayerController.action.InLevel.Attack.started += Launch;
        inputRangeTest.started += Launch;
    }

    private void Start()
    {
        AssignAmmoType();
        Initialize();
        _ammoStartY = _ammoRigidBody.transform.position.y;
        Invoke("SetLaunchPower", 0.1f);
        
    }


    /// <summary>
    /// Fýrlatma animasyonunu tetikle
    /// </summary>
    private void LaunchAnimTrigger()
    {
        _animator.SetTrigger("launch");
    }

    /// <summary>
    /// Fýrlatma iþlemini baþlatýr
    /// </summary>
    /// <param name="context"></param>
    protected override void Launch()
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
            
            _ammoRigidBody.AddForce(_ammoRigidBody.transform.up * _currentLaunchPower * height);
            _ammoRigidBody.transform.parent = null;
        }
        OnLaunchedInvoke();
    }
    
}
