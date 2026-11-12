using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{
    [Header("Buttons")]
    public Button playBtn;
    public Button settingsBtn;
    public Button exitBtn;

    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip hoverSound;

    void Start()
    {
    }

    public void OnPlayClicked()
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        SceneManager.LoadScene(1);
    }

    public void OnExitClicked()
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        Application.Quit();
    }
}
