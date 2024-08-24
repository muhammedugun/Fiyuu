// Refactor 24.08.24
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Fýrlatma inputu ile ilgili görevlerden sorumlu
/// </summary>
public class ThrowInputController : MonoBehaviour, IPointerDownHandler
{
    public static event Action Started;
    public bool isEnabled;

    [SerializeField] private Button _pauseButton, _resumeButton;
    [SerializeField] private RectTransform _pausePopUpTransform;
    [SerializeField] private Text _levelTitle;

    private void Start()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        _levelTitle.text = "Level " + levelIndex;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isEnabled)
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
    /// Ekrana týklandýðýnda gerçekleþerek olaylarý çaðýrýr.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(isEnabled)
            Started?.Invoke();
    }

    /// <summary>
    /// Fýrlatma kontrolünün açýklýk kapalýlýk durumunu ayarlar
    /// </summary>
    /// <param name="condition"></param>
    public void SetIsEnabled(bool condition)
    {
        isEnabled = condition;
    }
}
