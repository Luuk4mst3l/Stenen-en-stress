using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private string resourcePath = "level_1_dialogues";
    [SerializeField] private DialogueUIController ui;

    private LevelDialogueData data;
    private int index;
    private bool waitingForChoice;

    private void Start()
    {
        data = DialogueJsonLoader.LoadLevelFromResources(resourcePath);
        if (data == null) return;

        ui.OnOptionSelected += HandleOptionSelected;
        ui.OnTimedOut += HandleTimedOut;

        index = 0;
        StartCoroutine(RunEvents());
    }

    public void SkipDialogueIndex()
    {
        nextDialogueIndex++;
        Debug.Log($"DialogueRunner: Skipped to dialogue index {nextDialogueIndex}");
    }

    public void Activate()
    {
        if (data.events == null || data.events.Length == 0)
            yield break;

        while (index < data.events.Length)
        {
            // wait between popups
            yield return new WaitForSeconds(data.popupDelaySeconds);

            waitingForChoice = true;

            // show popup + start bar countdown (use popupDelaySeconds as "time to choose")
            ui.Show(data.events[index], data.popupDelaySeconds);

            while (waitingForChoice)
                yield return null;

            index++;
        }
    }

    private void HandleOptionSelected(DialogueOption opt)
    {
        Debug.Log($"Selected: {opt.id}");
        ui.Hide();
        waitingForChoice = false;
    }

    private void HandleTimedOut()
    {
        Debug.Log("Timed out");
        ui.Hide();
        waitingForChoice = false;
    }
}
