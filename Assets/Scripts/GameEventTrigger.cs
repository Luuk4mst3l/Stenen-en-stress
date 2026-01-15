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
}
