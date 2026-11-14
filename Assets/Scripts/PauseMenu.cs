using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject pauseCanvas;

    [Header("Buttons")]
    public Button resumeBtn;
    public Button settingsBtn;
    public Button quitBtn;

    [Header("Text")]
    public TextMeshProUGUI resumeText;
    public TextMeshProUGUI settingsText;
    public TextMeshProUGUI quitText;

    [Header("Input")]
    public InputAction pauseAction;

    [Header("Cursor")]
    public Texture2D cursorTexture;

    private bool isPauseMenuOpen = false;

    void Start()
    {
        ApplyTheme();
    }

    void TogglePauseMenu(InputAction.CallbackContext context)
    {
        Debug.Log("pause menu was toggled");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1)
        {
            isPauseMenuOpen = !isPauseMenuOpen;

            pauseCanvas.SetActive(isPauseMenuOpen);

            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(cursorTexture, Camera.main.transform.position, CursorMode.Auto);
        }
    }

    void ApplyTheme()
    {
        Debug.Log("applying theme");
        if (resumeBtn != null)
        {
            ColorBlock cb = resumeBtn.colors;
            cb.normalColor = new Color(0.99f, 0.88f, 0.28f, 1f);
            cb.highlightedColor = new Color(1.00f, 0.94f, 0.54f, 1f);
            cb.pressedColor = new Color(0.92f, 0.70f, 0.03f, 1f);
            cb.selectedColor = new Color(1.00f, 0.94f, 0.54f, 1f);
            cb.colorMultiplier = 1f;
            resumeBtn.colors = cb;
        }

        if (resumeText != null) resumeText.color = new Color(0.11f, 0.10f, 0.09f, 1f);

        if (settingsBtn != null)
        {
            ColorBlock cb = settingsBtn.colors;
            cb.normalColor = new Color(0.23f, 0.32f, 0.43f, 1f);
            cb.highlightedColor = new Color(0.28f, 0.40f, 0.53f, 1f);
            cb.pressedColor = new Color(0.16f, 0.23f, 0.31f, 1f);
            cb.selectedColor = new Color(0.28f, 0.40f, 0.53f, 1f);
            cb.colorMultiplier = 1f;
            settingsBtn.colors = cb;
        }

        if (settingsBtn != null) settingsText.color = new Color(0.88f, 0.95f, 1.00f, 1f);

        if (quitBtn != null)
        {
            ColorBlock cb = quitBtn.colors;
            cb.normalColor = new Color(0.36f, 0.11f, 0.14f, 1f);
            cb.highlightedColor = new Color(0.53f, 0.11f, 0.15f, 1f);
            cb.pressedColor = new Color(0.23f, 0.07f, 0.09f, 1f);
            cb.selectedColor = new Color(0.53f, 0.11f, 0.15f, 1f);
            cb.colorMultiplier = 1f;
            quitBtn.colors = cb;
        }

        if (quitText != null) quitText.color = new Color(1.00f, 0.80f, 0.83f, 1f);
    }

    void OnEnable()
    {
        pauseAction.Enable();

        pauseAction.performed += TogglePauseMenu;
    }

    void OnDisable()
    {
        pauseAction.Disable();

        pauseAction.performed -= TogglePauseMenu;
    }
}
