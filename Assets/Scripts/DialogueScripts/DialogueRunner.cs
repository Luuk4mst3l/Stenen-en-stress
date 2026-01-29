using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField]
    private string resourcePath = "level_1_dialogues";

    [SerializeField]
    private DialogueUIController ui;

    [SerializeField]
    private GameEventTracker gameEventTracker;

    private LevelDialogueData data;
    private int nextDialogueIndex = 0;
    private bool waitingForChoice;

    private void Start()
    {
        data = DialogueJsonLoader.LoadLevelFromResources(resourcePath);
        if (data != null)
        {
            ui.OnOptionSelected += HandleOptionSelected;
            ui.OnTimedOut += HandleTimedOut;
        }

        gameEventTracker.SetActiveDialogue(data);
    }

    public void Activate()
    {
        if (data == null || nextDialogueIndex >= data.events.Length)
            return;

        waitingForChoice = true;
        ui.Show(data.events[nextDialogueIndex], data.popupDelaySeconds);
    }

    private void HandleOptionSelected(DialogueOption opt)
    {
        ui.Hide();
        waitingForChoice = false;
        nextDialogueIndex++;
    }

    private void HandleTimedOut()
    {
        ui.Hide();
        waitingForChoice = false;
        nextDialogueIndex++;
    }

    private void OnDestroy()
    {
        if (ui != null)
        {
            ui.OnOptionSelected -= HandleOptionSelected;
            ui.OnTimedOut -= HandleTimedOut;
        }
    }
}
