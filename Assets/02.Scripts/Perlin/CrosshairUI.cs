using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public RectTransform crosshairRect;

    public bool hideCursor = true;
    public bool lockCursor = true;

    private void Awake()
    {
        if (crosshairRect == null)
        {
            crosshairRect = GetComponent<RectTransform>();
        }

        if (crosshairRect != null)
        {
            crosshairRect.anchorMin = new Vector2(0.5f, 0.5f);
            crosshairRect.anchorMax = new Vector2(0.5f, 0.5f);
            crosshairRect.pivot = new Vector2(0.5f, 0.5f);
            crosshairRect.anchoredPosition = Vector2.zero;
        }

        ApplyCursorState();
    }

    public void SetCursorStatus(bool isUIOpen)
    {
        if (isUIOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ApplyCursorState();
        }
    }

    void ApplyCursorState()
    {
        if (hideCursor)
        {
            Cursor.visible = false;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
