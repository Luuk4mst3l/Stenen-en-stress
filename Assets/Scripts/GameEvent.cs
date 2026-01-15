using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    [Header("Event Metadata")]
    public string eventId;
    [TextArea]
    public string description;

    [Header("Event Actions")]
    public UnityEvent onEventTriggered;

    [Header("Dialogue Options")]
    public List<DialogueOption> dialogueOptions;
}
