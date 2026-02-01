using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField]
    private string resourcePath = "level_1_dialogues";

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip signalClip;

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

    private bool hasNotifiedCurrent = false;
    private float nextNotifyTime = 0f;
    private int totalDialogues;
    private int passedDialogues;

    private void Start()
    {
        data = DialogueJsonLoader.LoadLevelFromResources(resourcePath);
        if (data != null)
        {
            ui.OnOptionSelected += HandleOptionSelected;
            ui.OnTimedOut += HandleTimedOut;
        }

        gameEventTracker.SetActiveDialogue(data);
        totalDialogues = data.events.Length;
        passedDialogues = 0;

        nextNotifyTime = Time.time + initialDialogueDelay;
        hasNotifiedCurrent = false;
    }

    private void FixedUpdate()
    {
        if (data == null || gameEventTracker == null)
            return;

        if (waitingForChoice)
            return;

        if (nextDialogueIndex >= data.events.Length)
            return;

        if (hasNotifiedCurrent)
            return;

        if (Time.time >= nextNotifyTime)
        {
            int stakeholderIndex = data.events[nextDialogueIndex].partij;
            gameEventTracker.NotifyStakeholderForDialogue(stakeholderIndex);

            hasNotifiedCurrent = true;
        }
    }

    public bool checkFailedDialogue()
    {
        if (passedDialogues < totalDialogues)
        {
            return true;
        }
        else
        {
            return false;
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
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource != null && signalClip != null)
            audioSource.PlayOneShot(signalClip, 1f);

        hasNotifiedCurrent = false;
        nextNotifyTime = Time.time + data.popupDelaySeconds;

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
        passedDialogues++;

        hasNotifiedCurrent = false;
        nextNotifyTime = Time.time + data.popupDelaySeconds;
    }

    private void HandleTimedOut()
    {
        ui.Hide();
        waitingForChoice = false;
        nextDialogueIndex++;

        hasNotifiedCurrent = false;
        nextNotifyTime = Time.time + data.popupDelaySeconds;
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
