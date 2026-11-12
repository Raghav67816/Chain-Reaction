using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu: MonoBehaviour
{

    [Header("Buttons")]
    public Button playBtn;
    public Button settingsBtn;
    public Button exitBtn;

    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public AudioClip samMessage;

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPlayClicked()
    {
        audioSource.PlayOneShot(clickSound);
        StartCoroutine(PlayMessage());
    }

    IEnumerator PlayMessage()
    {
        audioSource.PlayOneShot(samMessage);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(1);
    }

    public void OnExitClicked()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }

}
