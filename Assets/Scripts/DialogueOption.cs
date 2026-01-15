using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueOption
{
    [Header("UI")]
    public string optionId;
    [TextArea]
    public string dialogueText;

    [Header("Consequences")]
    public int reputationChange;
    public int goldChange;

    public UnityEvent onOptionSelected;

    private void hello()
  {
    // Debug.Log("Hello from DialogueOption!");
  }
}