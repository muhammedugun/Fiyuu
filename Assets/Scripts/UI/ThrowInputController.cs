// Refactor 24.08.24
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// F�rlatma inputu ile ilgili g�revlerden sorumlu
/// </summary>
public class ThrowInputController : MonoBehaviour, IPointerDownHandler
{
    public static event Action Started;
    public bool isEnabled;

    [SerializeField] private Button _pauseButton, _resumeButton;
    [SerializeField] private RectTransform _pausePopUpTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isEnabled)
            Started?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausePopUpTransform.gameObject.activeSelf)
                _pauseButton.onClick.Invoke();
            else
                _resumeButton.onClick.Invoke();
        }
    }

    /// <summary>
    /// Ekrana t�kland���nda ger�ekle�erek olaylar� �a��r�r.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEnabled)
            Started?.Invoke();
    }

    /// <summary>
    /// F�rlatma kontrol�n�n a��kl�k kapal�l�k durumunu ayarlar
    /// </summary>
    /// <param name="condition"></param>
    public void SetIsEnabled(bool condition)
    {
        isEnabled = condition;
    }
}
