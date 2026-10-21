using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu: MonoBehaviour
{
    [Header("UI Document")]
    public UIDocument uIDocument;
    
    // Buttons
    private Button playBtn;
    private Button settingsBtn;
    private Button exitBtn;

    private bool btnsFound = false;

    void Start()
    {
        if(uIDocument != null)
        {
            Debug.Log("Document found");
            VisualElement root = uIDocument.rootVisualElement;

            playBtn = root.Q<Button>("StartButton");
            settingsBtn = root.Q<Button>("SettingsButton");
            exitBtn = root.Q<Button>("ExitButton");

            if(playBtn != null && settingsBtn != null && exitBtn != null)
            {
                btnsFound = true;
                playBtn.RegisterCallback<ClickEvent>(OnPlayClicked);
                exitBtn.RegisterCallback<ClickEvent>(OnExitClicked);
            }
        }
    }

    void OnPlayClicked(ClickEvent evnt)
    {
        BeepSound.Instance.PlayAudio();
        Debug.Log("Play clicked");
    }

    void OnExitClicked(ClickEvent evnt)
    {
        Application.Quit();
    }

    void OnDisable()
    {
        if (btnsFound)
        {
            playBtn.UnregisterCallback<ClickEvent>(OnPlayClicked);
            exitBtn.UnregisterCallback<ClickEvent>(OnExitClicked);
        }
    }
}
