using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class TerminalLogger : MonoBehaviour
{

    public TextAsset terminalContent;
    public TextMeshProUGUI textField;
    private String[] logs;

    [Header("Duration")]
    public float maxWaitTime = 0.5f;
    public float minWaitTime = 1f;

    void Start()
    {
        if(terminalContent != null && textField != null)
        {
            logs = terminalContent.text.Split("\n");
            foreach(var line in logs)
            {
                Debug.Log(line);
            }

            StartCoroutine(TerminalAnimation());
        }
    }

    IEnumerator TerminalAnimation()
    {
        int idx = 0;
        while(idx < logs.Count())
        {
            textField.text += "\n" + logs[idx];
            BeepSound.Instance.PlayAudio();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));

            idx++;
        }
    }
}
