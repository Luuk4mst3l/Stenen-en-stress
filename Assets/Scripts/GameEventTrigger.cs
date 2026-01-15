using System.Collections;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    public void TriggerEvent()
    {
        if (gameEvent == null) return;

        gameEvent.onEventTriggered.Invoke();

        foreach (var item in gameEvent.dialogueOptions)
        {
            Debug.Log("Dialogue Option: " + item.dialogueText);
        }
    }

    // New method for timed dialogue triggering
    public void StartTimedDialogue(float delay, DialogueOption option)
    {
        StartCoroutine(TriggerAfterDelay(delay, option));
    }

    private IEnumerator TriggerAfterDelay(float delay, DialogueOption option)
    {
        yield return new WaitForSeconds(delay);
        // Assuming DialogueManager is accessible
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.ShowDialogue(option);
        }
    }
}
