using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blobcreate.ProjectileToolkit;

public class ProjectileTest : MonoBehaviour
{
    [Header("Render")]
    [SerializeField] Transform launchPosition;
    [SerializeField] float distanceOrEnd;
    private Vector3 launchVelocity;

    [Header("Fýrlatma")]
    [SerializeField] TrajectoryPredictor tp;
    public Transform target;
    [SerializeField] Rigidbody mermiPrefab;
    [SerializeField] float timeOfFlight;




    private void Update()
    {
        launchVelocity = Blobcreate.ProjectileToolkit.Projectile.VelocityByTime(launchPosition.position, target.position, timeOfFlight);
        tp.Render(launchPosition.position, launchVelocity, distanceOrEnd);
        if (Input.GetKeyDown(KeyCode.Space))
            MyMethod();
    }

    void MyMethod()
    {
        var mermi = Instantiate(mermiPrefab, launchPosition.position, launchPosition.rotation);
        mermi.AddForce(launchVelocity, ForceMode.VelocityChange);
    }
    
}
