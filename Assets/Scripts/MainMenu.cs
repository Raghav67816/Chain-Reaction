using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainMenu: MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("main menu enabled");

    }

    void Osable()
    {
        Debug.Log("main menu disabled.");
    }

    void onUIReload(PanelRenderer panelRenderer, VisualElement rootElement, int version)
    {
    }
}
