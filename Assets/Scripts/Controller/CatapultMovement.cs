using Cinemachine;
using UnityEngine;

public class CatapultMovement : MonoBehaviour
{
    /// <summary>
    /// katapultun fýrlatýcý parçasý
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


    // Mouse hareketinin önceki frame'deki konumunu saklamak için bir deðiþken.
    private Vector3 _previousMousePosition;

    // Hýzý hesaplamak için bir deðiþken.
    private float _speed;
    /// <summary>
    /// Katapultun fýrlatýcý (launcher) parçasýný hareket ettirir.
    /// </summary>
    private void LauncherPartMove()
    {

        // Mouse'un y eksenindeki hareketini yakalama
        float mouseY = Input.GetAxis("Mouse Y");

        // Z rotasyonunu mouse hareketine göre ayarlama
        _zRotation += mouseY * Time.deltaTime * 100;

        // Z rotasyonunu 0f ila 58f arasýnda sýnýrlama
        _zRotation = Mathf.Clamp(_zRotation, 0f, 58f);

        // Gameobject'in rotasyonunu güncelleme
        _launcherPartRB.transform.rotation = Quaternion.Euler(_launcherPartRB.transform.rotation.eulerAngles.x, _launcherPartRB.transform.rotation.eulerAngles.y, _zRotation);

        // Mouse hareketinin anlýk hýzýný hesaplama
        _speed = Vector3.Distance(Input.mousePosition, _previousMousePosition) / Time.deltaTime;

        // Mouse'un önceki frame'deki konumunu güncelleme
        _previousMousePosition = Input.mousePosition;

    }


    [SerializeField] private Catapult _catapult;
    /// <summary>
    /// Mühimmat fýrlat
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

