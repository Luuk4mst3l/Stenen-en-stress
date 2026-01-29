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

    [SerializeField]
    private float initialDialogueDelay = 10f;

    private LevelDialogueData data;
    private int nextDialogueIndex = 0;
    private bool waitingForChoice;
    private float elapsedTime = 0f;

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

    private void FixedUpdate()
    {
        if (gameEventTracker != null && data != null && !waitingForChoice)
        {
            elapsedTime += Time.fixedDeltaTime;
            UpdateDialogueIntervals();
        }
    }

    private void UpdateDialogueIntervals()
    {
        if (nextDialogueIndex >= data.events.Length)
            return;

        float unlockTime = initialDialogueDelay + nextDialogueIndex * data.popupDelaySeconds;

        if (elapsedTime >= unlockTime)
        {
            int stakeholderIndex = data.events[nextDialogueIndex].partij;
            gameEventTracker.NotifyStakeholderForDialogue(stakeholderIndex);
        }
    }

    public void SkipDialogueIndex()
    {
        Debug.Log("Skipping dialogue index " + nextDialogueIndex);
        nextDialogueIndex++;
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
