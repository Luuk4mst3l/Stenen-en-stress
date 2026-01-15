using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // Assign a UI Panel in Inspector
    [SerializeField] private Text dialogueText; // Assign a Text component for the dialogue
    [SerializeField] private Button[] optionButtons; // Assign buttons for options (e.g., 2-3 buttons)

    private DialogueOption currentOption;

    public void ShowDialogue(DialogueOption option)
    {
        currentOption = option;
        dialoguePanel.SetActive(true);
        dialogueText.text = option.dialogueText;

        // Assuming 2 options; adjust as needed
        if (optionButtons.Length > 0 && option.onOptionSelected != null)
        {
            optionButtons[0].onClick.AddListener(() => SelectOption(option));
        }
    }

    private void SelectOption(DialogueOption option)
    {
        option.onOptionSelected.Invoke();
        dialoguePanel.SetActive(false);
        // Apply consequences (e.g., reputationChange, goldChange)
    }
}