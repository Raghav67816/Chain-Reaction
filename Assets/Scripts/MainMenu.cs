using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{

    [Header("Drone")]
    public Drone drone;

    [Header("Buttons")]
    public Button playBtn;
    public Button settingsBtn;
    public Button exitBtn;

    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public AudioClip samMessage;

    [Header("Cursor")]
    public Texture2D cursor;

    void Start()
    {
        drone.StopDrone();
        Cursor.SetCursor(cursor, Camera.main.transform.position, CursorMode.Auto);
    }

    void FixedUpdate()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        if(!Mathf.Approximately(mouseWorldPos.magnitude, drone.droneBody.transform.position.magnitude))
        {
            drone.droneBody.MovePosition(mouseWorldPos);
        }
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPlayClicked()
    {
        playBtn.enabled = false;
        audioSource.PlayOneShot(clickSound);
        StartCoroutine(PlayMessage());
    }

    IEnumerator PlayMessage()
    {
        audioSource.PlayOneShot(samMessage);
        yield return new WaitForSeconds(samMessage.length);
        SceneManager.LoadScene(1);
    }

    public void OnExitClicked()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }

}
