using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairFollow : MonoBehaviour
{
    public RectTransform crosshairSprite;
    public Canvas canvas;

    public float sensitivity = 1f;

    private Vector2 currentPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentPosition = Vector2.zero;
        crosshairSprite.anchoredPosition = currentPosition;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        currentPosition += delta * sensitivity;

        RectTransform canvasRect = canvas.transform as RectTransform;

        currentPosition.x = Mathf.Clamp(
            currentPosition.x,
            canvasRect.rect.xMin,
            canvasRect.rect.xMax
        );

        currentPosition.y = Mathf.Clamp(
            currentPosition.y,
            canvasRect.rect.yMin,
            canvasRect.rect.yMax
        );

        crosshairSprite.anchoredPosition = Vector3.Lerp(
            crosshairSprite.anchoredPosition,
            currentPosition,
            2f * Time.deltaTime
        );
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !hasFocus;
    }
}