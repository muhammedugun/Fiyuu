// Refactor 09.05.24
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
    [SerializeField] private Button pauseButton, resumeButton;
    [SerializeField] private RectTransform _pausePopUpTransform;
    [SerializeField] private Text levelTitle;

    private void Start()
    {
        int levelIndex = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        levelTitle.text = "Level " + levelIndex;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isEnabled)
            Started?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausePopUpTransform.gameObject.activeSelf)
                pauseButton.onClick.Invoke();
            else
                resumeButton.onClick.Invoke();

        }
            
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isEnabled)
            Started?.Invoke();
    }

    public void SetIsEnabled(bool condition)
    {
        isEnabled = condition;
    }
}
