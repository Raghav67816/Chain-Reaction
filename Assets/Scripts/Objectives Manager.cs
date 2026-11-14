using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    private Dictionary<int, Objective> objectives;

    [Header("Canvas Settings")]
    public Canvas canvas;
    
    [Header("Objective Font Settings")]
    public FontStyles fontStyle;
    public int fontSize;
    public Color incompleteColor;
    public Color completeColor;

    // events
    public static event Action<Objective> onNewObjective;

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

        TextMeshProUGUI obj = new TextMeshProUGUI
        {
            text = text,
            fontStyle = fontStyle,
            fontSize = fontSize,
            color = incompleteColor
        };

        Objective objective = new Objective(obj);
        objectives.Add(id, objective);

        UpdateObjectiveInUI(objective);

        onNewObjective.Invoke(objective);
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
}
