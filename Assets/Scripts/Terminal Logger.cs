using System;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalLogger : MonoBehaviour
{

    public TextAsset terminalContent;
    public TextMeshProUGUI textField;
    public TextMeshProUGUI malfunctionText;


    private string[] logs;
    private int randomIdx = 2;
    private bool isMalfunctioning = false;
    private int logIndex = 0;

    [Header("Duration")]
    public float maxWaitTime = 1f;
    public float minWaitTime = 0.5f;

    void Start()
    {
        if (terminalContent != null && textField != null && malfunctionText != null)
        {
            logs = terminalContent.text.Split('\n');
            foreach (var line in logs)
            {
                Debug.Log(line);
            }

            randomIdx = UnityEngine.Random.Range(2, logs.Count() - 2);
            StartCoroutine(TerminalAnimation());
        }
    }

    IEnumerator TerminalAnimation()
    {
        logIndex = 0;
        while (logIndex < logs.Length)
        {
            if (logIndex == randomIdx && !isMalfunctioning)
            {
                yield return StartCoroutine(Malfunction());
            }

            textField.text += "\n" + logs[logIndex];

            if(BeepSound.Instance != null)
            {
                BeepSound.Instance.PlayAudio();
            }   

            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTime));

            logIndex++;
        }
    }

    IEnumerator Malfunction()
    {
        isMalfunctioning = true;
        textField.enabled = false;

        Color redColor = new Color(1f, 0f, 0f);

        textField.color = redColor;

        UnityEngine.WaitForSeconds waitTime = new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTime));

        yield return waitTime;
        textField.enabled = true;

        if(logIndex >= logs.Length)
        {
            textField.enabled = false;
        }

        malfunctionText.enabled = true;

        SceneManager.LoadSceneAsync(2);
    }
}
