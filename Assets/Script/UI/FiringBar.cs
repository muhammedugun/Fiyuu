using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Ate�leme bar� (Catapult ate�lendi�inde dolan bar) ile ilgili
/// </summary>
public class FiringBar : MonoBehaviour
{
    [Tooltip("Dolan ve bo�alan bar'� temsil eden Image bile�eni")]
    [SerializeField] private Image _firingBarImage;
    [Tooltip("Bar'�n doldurma h�z�")]
    [SerializeField] private float _fillSpeed = 1.0f;
    /// <summary>
    /// �u anki dolma miktar�
    /// </summary>
    internal float currentFillAmount = 0.0f;
    /// <summary>
    /// Dolma i�lemi devam ediyor mu?
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
    /// Doldurma i�lemini ba�lat�r
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
    /// Bar�n doldurulabilirli�ini kontrol et 
    /// </summary>
    /// <returns></returns>
    private bool CheckFilling()
    {
        if (_isFilling == true && currentFillAmount < 1f) return true; else return false;
    }

    /// <summary>
    /// Bar�n bo�alt�mas�n� ba�lat
    /// </summary>
    void StartEmpyting()
    {
        if (_isFilling == true)
            _isFilling = false;
    }

    /// <summary>
    /// Bar� bo�altmaya devam et
    /// </summary>
    void KeepEmpyting()
    {
        currentFillAmount -= _fillSpeed * Time.deltaTime;
        _firingBarImage.fillAmount = currentFillAmount;
    }

    /// <summary>
    /// Bar�n bo�alt�labilirli�ini kontrol et
    /// </summary>
    /// <returns></returns>
    private bool CheckEmpyting()
    {
        if (_isFilling == false && currentFillAmount > 0) return true; else return false;
    }


}
