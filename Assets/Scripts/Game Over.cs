using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("Trigger Boundary")]
    public Collider2D boundary;

    [Header("Game Over Canvas")]
    public GameObject gameOverCanvas;

    private LayerMask playerLayer;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            Time.timeScale = 0f;
            gameOverCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
