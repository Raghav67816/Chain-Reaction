using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject inventoryHotbar;

    void OnEnable()
    {
        HRCCollider.OnPlayerInHRC += OnPlayerInHRC;
    }

    void Start()
    {
        inventoryHotbar.SetActive(false);
    }

    void OnPlayerInHRC(int a)
    {
        inventoryHotbar.SetActive(true);
    }
}
