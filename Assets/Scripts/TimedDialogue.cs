using UnityEngine;

[System.Serializable]
public class TimedDialogue
{
    public float triggerTime; // Time in seconds when to trigger
    public DialogueOption dialogueOption; // The dialogue to show
    public bool hasTriggered = false; // Track if it's been triggered
}