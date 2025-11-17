using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("크로스헤어 이미지")]
    public RectTransform crosshairRect;   // 크로스헤어 이미지 RectTransform

    [Header("옵션")]
    public bool hideCursor = true;        // 마우스 커서 숨길지 여부
    public bool lockCursor = true;        // 마우스 커서를 중앙에 고정할지 여부

    private void Awake()
    {
        if (crosshairRect == null)
        {
            // 이 스크립트가 붙은 오브젝트에 RectTransform이 있으면 자동으로 가져오기
            crosshairRect = GetComponent<RectTransform>();
        }

        // 안전장치: 크로스헤어가 화면 중앙에 오도록 앵커/좌표 강제 세팅
        if (crosshairRect != null)
        {
            crosshairRect.anchorMin = new Vector2(0.5f, 0.5f);
            crosshairRect.anchorMax = new Vector2(0.5f, 0.5f);
            crosshairRect.pivot = new Vector2(0.5f, 0.5f);
            crosshairRect.anchoredPosition = Vector2.zero;
        }

        ApplyCursorState();
    }

    private void OnEnable()
    {
        ApplyCursorState();
    }

    private void OnDisable()
    {
        // 비활성화되면 커서 다시 돌려놓기 (원하면 생략 가능)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
