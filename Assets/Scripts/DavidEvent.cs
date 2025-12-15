using System;
using Unity.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DavidEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject dialoguePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + gameObject.name);
        OpenDialogue();
    }

    private void OpenDialogue()
    {
        if (!dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
