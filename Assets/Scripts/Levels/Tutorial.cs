using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [Header("Text Transforms")]
    public GameObject welcome;
    
    void Start()
    {
        ShowWelcomeText();
    }

    void ShowWelcomeText()
    {
        TextMeshProUGUI welcomeText = welcome.AddComponent<TextMeshProUGUI>();
        welcomeText.text = "WELCOME TO THE GAME";
        welcomeText.fontSize = 14;
    }
}
