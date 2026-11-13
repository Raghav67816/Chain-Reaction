using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Bounds")]
    public Transform left;
    public Transform right;

    private Vector2 newDronePos;
    private FoodHoverAnimation hoverAnimation;

    void Start()
    {
        hoverAnimation = drone.gameObject.GetComponent<FoodHoverAnimation>();

        Cursor.SetCursor(cursor, Camera.main.transform.position, CursorMode.Auto);
        StartCoroutine(FindNewPos());
    }

    void FixedUpdate()
    {
        Vector2 smoothPos = Vector2.MoveTowards(
            drone.droneBody.position,
            newDronePos,
            4f * Time.fixedDeltaTime
        );

        drone.droneBody.MovePosition(smoothPos);

        if(drone.droneBody.linearVelocity.magnitude < 0.5f)
        {
            hoverAnimation.enabled = true;
        }

        else
        {
            hoverAnimation.enabled = false;
        }
    }

    IEnumerator FindNewPos()
    {
        while (true)
        {
            Vector2 newPos = new Vector2(
                Random.Range(left.transform.position.x, right.transform.position.x),
                Random.Range(left.transform.position.y, right.transform.position.y)
            );

            newDronePos = newPos;

            yield return new WaitForSeconds(5f);
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
