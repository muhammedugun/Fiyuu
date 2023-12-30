using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Ate�leme bar� (Catapult ate�lendi�inde dolan bar) ile ilgili
/// </summary>
public class FiringBar : MonoBehaviour
{
    [Inject] InputRangeTest inputRangeTest;
    /// <summary>
    /// Dolan ve bo�alan bar'� temsil eden Image bile�eni
    /// </summary>
    private Image _firingBarImage;
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
        _firingBarImage = GetComponent<Image>();
        //PlayerController.action.InLevel.Attack.started += StartFilling;
        inputRangeTest.started += StartFilling;
        inputRangeTest.performed += BarControl;
    }

    void BarControl()
    {
        if (CheckFilling())
        {
            KeepFilling();
        }
        else
        {
            StartEmpyting();
            if (CheckEmpyting())
            {
                KeepEmpyting();
            }
            else
            {
                StartFilling();
            }
        }
    }

    /// <summary>
    /// Doldurma i�lemini ba�lat�r
    /// </summary>
    /// <param name="context"></param>
    void StartFilling()
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
        if(_firingBarImage==null)
        {
            _firingBarImage = GetComponent<Image>();
        }
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
