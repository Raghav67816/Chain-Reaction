using TMPro;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct Objective
{
    public TextMeshProUGUI text;
    public bool isComplete;

    public Objective(TextMeshProUGUI objectiveText)
    {
        text = objectiveText;
        isComplete = false;
    }
}

public class ObjectivesManager : MonoBehaviour
{
    // starting positions for objectives
    private int startX = 50;
    private int startY = -25;
    private Dictionary<int, Objective> objectives = new Dictionary<int, Objective>();

    [Header("Canvas Settings")]
    public Canvas canvas;
    
    [Header("Objective Font Settings")]
    public FontStyles fontStyle;
    public int fontSize;
    public Color incompleteColor;
    public Color completeColor;

    // events
    public static event Action<Objective> OnNewObjective;

    void Start()
    {
        StartCoroutine(AddObjective());
    }

    /*
    a note for me...

    each objective must have a unique id
    and it is the responsibility of dev (ofc me) to mark them complete
     */

    public void NewObjective(int id, string text)
    {
        if (objectives.ContainsKey(id))
        {
            Debug.LogWarning($"Objective already exists with id: {id}");
            return;
        }

        GameObject objGame = new GameObject(text, typeof(RectTransform));
        objGame.transform.SetParent(canvas.transform, false);

        TextMeshProUGUI obj = objGame.AddComponent<TextMeshProUGUI>();

        obj.text = text;
        obj.fontStyle = fontStyle;
        obj.fontSize = fontSize;
        obj.color = incompleteColor;

        Objective objective = new Objective(obj);
        objectives.Add(id, objective);

        UpdateObjectiveInUI(objective);

        OnNewObjective?.Invoke(objective);
    }

    private void UpdateObjectiveInUI(Objective obj)
    {
        Instantiate(
            obj.text,
            new Vector3(
                canvas.transform.position.x + (startX * objectives.Count),
                canvas.transform.position.y + (startY * objectives.Count),
                0f
            ),
            Quaternion.identity
        );
    }

    public void OnObjectiveComplete(int id)
    {
        Objective obj;
        if(objectives.TryGetValue(id, out obj))
        {
            obj.isComplete = true;
            UpdateObjectiveInUI(obj);
        }
    }

    // for development only
    IEnumerator AddObjective()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1f);
        for(int i=0; i<5; i++)
        {
            Debug.Log(i);
            yield return waitTime;
        }

        NewObjective(23, "Hello");
    }
}
