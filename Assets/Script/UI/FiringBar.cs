using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Ateþleme barý (Catapult ateþlendiðinde dolan bar) ile ilgili
/// </summary>
public class FiringBar : MonoBehaviour
{
    [Tooltip("Dolan ve boþalan bar'ý temsil eden Image bileþeni")]
    [SerializeField] private Image _firingBarImage;
    [Tooltip("Bar'ýn doldurma hýzý")]
    [SerializeField] private float _fillSpeed = 1.0f;
    /// <summary>
    /// Þu anki dolma miktarý
    /// </summary>
    internal float currentFillAmount = 0.0f;
    /// <summary>
    /// Dolma iþlemi devam ediyor mu?
    /// </summary>
    private bool _isFilling = false;

    private void Start()
    {
        PlayerController.action.InLevel.Attack.started += StartFilling;
    }

    private void Update()
    {
        if (PlayerController.isPerformed)
        {
            if (CheckFilling())
            {
                KeepFilling();
            }
            else
            {
                StartEmpyting();
                if(CheckEmpyting())
                {
                    KeepEmpyting();
                }
                else
                {
                    StartFilling(new InputAction.CallbackContext());
                }
            }
        }
    }

    /// <summary>
    /// Doldurma iþlemini baþlatýr
    /// </summary>
    /// <param name="context"></param>
    void StartFilling(InputAction.CallbackContext context)
    {
        _isFilling = true;
        currentFillAmount = 0.0f;
    }

    /// <summary>
    /// Doldurmaya devam et
    /// </summary>
    void KeepFilling()
    {
        currentFillAmount += _fillSpeed * Time.deltaTime;
        _firingBarImage.fillAmount = currentFillAmount;
    }

    /// <summary>
    /// Barýn doldurulabilirliðini kontrol et 
    /// </summary>
    /// <returns></returns>
    private bool CheckFilling()
    {
        if (_isFilling == true && currentFillAmount < 1f) return true; else return false;
    }

    /// <summary>
    /// Barýn boþaltýmasýný baþlat
    /// </summary>
    void StartEmpyting()
    {
        if (_isFilling == true)
            _isFilling = false;
    }

    /// <summary>
    /// Barý boþaltmaya devam et
    /// </summary>
    void KeepEmpyting()
    {
        currentFillAmount -= _fillSpeed * Time.deltaTime;
        _firingBarImage.fillAmount = currentFillAmount;
    }

    /// <summary>
    /// Barýn boþaltýlabilirliðini kontrol et
    /// </summary>
    /// <returns></returns>
    private bool CheckEmpyting()
    {
        if (_isFilling == false && currentFillAmount > 0) return true; else return false;
    }


}
