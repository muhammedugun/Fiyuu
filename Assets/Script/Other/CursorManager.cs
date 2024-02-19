using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        Hide();
        Subscribe();
    }

    private void Subscribe()
    {
        InLevelManager.OnGameOver += Show;
    }

    private void Hide()
    {
        Cursor.visible = false;
    }
    private void Show()
    {
        Cursor.visible = true;
    }
}
