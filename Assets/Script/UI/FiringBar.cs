using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FiringBar : MonoBehaviour
{
    [Tooltip("Dolan ve bo�alan bar'� temsil eden Image bile�eni")]
    [SerializeField] private Image _firingBarImage;
    [Tooltip("Bar'�n doldurma h�z�")]
    [SerializeField] private float _fillSpeed = 1.0f;
    internal float currentFillAmount = 0.0f; // �u anki dolma miktar�
    private bool _isFilling = false; // Dolma i�lemi devam ediyor mu?

    private void Start()
    {
        PlayerController.action.InLevel.Attack.started += FillingControl;
    }

    private void Update()
    {
        if (PlayerController.isPerformed)
        {
            FillBarcode();
        }
    }

    void FillingControl(InputAction.CallbackContext context)
    {
        _isFilling = true;
        currentFillAmount = 0.0f;
    }

    void FillBarcode()
    {
        Debug.Log("fillbarcode");
        if (_isFilling == true && currentFillAmount < 1f)
        {
            currentFillAmount += _fillSpeed * Time.deltaTime;
            _firingBarImage.fillAmount = currentFillAmount;
        }
        else
        {
            _isFilling=false;
            EmptyBarcode();
        }
    }

    void EmptyBarcode()
    {
        if (_isFilling == false && currentFillAmount > 0)
        {
            currentFillAmount -= _fillSpeed * Time.deltaTime;
            _firingBarImage.fillAmount = currentFillAmount;
        }
        else
        {
            _isFilling=true;
        }
    }
}
