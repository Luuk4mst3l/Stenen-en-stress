using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyStakeHolders : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField]
    private float timeToRespond = 5f;

    [SerializeField]
    private Image timerFill;

    [Header("Stakeholders to Notify")]
    [SerializeField]
    private List<StakeHolders> stakeholders = new List<StakeHolders>();

    [Header("Dialogue")]
    [SerializeField]
    private DialogueRunner dialogueRunner;

    private float elapsed;
    private bool resolved;

    private void OnEnable()
    {
        elapsed = 0f;
        resolved = false;
        StartCoroutine(ResponseTimer());
    }

    private IEnumerator ResponseTimer()
    {
        while (elapsed < timeToRespond)
        {
            elapsed += Time.deltaTime;

            if (timerFill != null)
                timerFill.fillAmount = 1f - (elapsed / timeToRespond);

            yield return null;
        }

        if (!resolved)
            OnTimedOut();
    }

    // Hook this once to the Button OnClick
    public void NotifySelectedStakeholders()
    {
        if (resolved)
            return;

        resolved = true;

        foreach (var stakeholder in stakeholders)
        {
            stakeholder?.Signal();
        }

        dialogueRunner?.StartDialogue();
        ClosePopup();
    }

    private void OnTimedOut()
    {
        Debug.Log("Stakeholder notification timed out");
        MissedConsequence();
        ClosePopup();
    }

    private void MissedConsequence()
    {
        // Placeholder – expand later
        Debug.Log("Apply missed stakeholder consequence here");
    }

    private void ClosePopup()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
