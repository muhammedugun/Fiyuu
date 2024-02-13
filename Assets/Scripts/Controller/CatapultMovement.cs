using Cinemachine;
using UnityEngine;

public class CatapultMovement : MonoBehaviour
{
    /// <summary>
    /// katapultun f�rlat�c� par�as�
    /// </summary>
    [SerializeField] private GameObject _launcherPart;
    private float _zRotation;

    private Rigidbody _launcherPartRB;
    private void Start()
    {
        _zRotation = _launcherPart.transform.rotation.z;
        _launcherPartRB = _launcherPart.GetComponent<Rigidbody>();
    }


    void Update()
    {
        LauncherPartMove();
        ThrowAmmo();
    }


    // Mouse hareketinin �nceki frame'deki konumunu saklamak i�in bir de�i�ken.
    private Vector3 _previousMousePosition;

    // H�z� hesaplamak i�in bir de�i�ken.
    private float _speed;
    /// <summary>
    /// Katapultun f�rlat�c� (launcher) par�as�n� hareket ettirir.
    /// </summary>
    private void LauncherPartMove()
    {

        // Mouse'un y eksenindeki hareketini yakalama
        float mouseY = Input.GetAxis("Mouse Y");

        // Z rotasyonunu mouse hareketine g�re ayarlama
        _zRotation += mouseY * Time.deltaTime * 100;

        // Z rotasyonunu 0f ila 58f aras�nda s�n�rlama
        _zRotation = Mathf.Clamp(_zRotation, 0f, 58f);

        // Gameobject'in rotasyonunu g�ncelleme
        _launcherPartRB.transform.rotation = Quaternion.Euler(_launcherPartRB.transform.rotation.eulerAngles.x, _launcherPartRB.transform.rotation.eulerAngles.y, _zRotation);

        // Mouse hareketinin anl�k h�z�n� hesaplama
        _speed = Vector3.Distance(Input.mousePosition, _previousMousePosition) / Time.deltaTime;

        // Mouse'un �nceki frame'deki konumunu g�ncelleme
        _previousMousePosition = Input.mousePosition;

    }


    [SerializeField] private Catapult _catapult;
    /// <summary>
    /// M�himmat f�rlat
    /// </summary>
    private void ThrowAmmo()
    {

        if (!_catapult.initialized && _launcherPart.transform.rotation.eulerAngles.z >= 57f && _speed>300f)
        {
            _catapult.initialized = true;
            if (_speed>=1500f)
            {
                _catapult.Launch(1500f / 7f);
            }
            else
            {
                _catapult.Launch(_speed / 7f);
            }
            
        }

    }


}

