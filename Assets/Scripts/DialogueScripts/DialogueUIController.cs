using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private Button option1Button;

    [SerializeField]
    private TMP_Text option1Label;

    [SerializeField]
    private Button option2Button;

    [SerializeField]
    private TMP_Text option2Label;

    [Header("Timer Bar (top image)")]
    [SerializeField]
    private Image timerFillImage;

    [SerializeField]
    private bool hideBarWhenNoTimer = true;

    [Header("Gameplay Wiring")]
    [SerializeField]
    private StakeholderRelationshipManager relationshipManager;

    [SerializeField]
    private PlayerWallet playerWallet;

    [Header("Timeout Behaviour")]
    [SerializeField]
    private bool autoPickFirstOptionOnTimeout = true;

    public event Action<DialogueOption> OnOptionSelected;
    public event Action OnTimedOut;

    private DialogueEvent current;

    private float timerDuration;
    private float timerRemaining;
    private bool timerRunning;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (relationshipManager == null)
            relationshipManager = FindFirstObjectByType<StakeholderRelationshipManager>();

        if (timerFillImage != null)
        {
            timerFillImage.type = Image.Type.Filled;
            timerFillImage.fillMethod = Image.FillMethod.Horizontal;
            timerFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            timerFillImage.fillAmount = 1f;
            if (hideBarWhenNoTimer)
                timerFillImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        timerRemaining -= Time.deltaTime;

        float t = (timerDuration <= 0f) ? 0f : Mathf.Clamp01(timerRemaining / timerDuration);
        SetBar(t);

        if (timerRemaining <= 0f)
        {
            timerRunning = false;
            HandleTimeout();
        }
    }

    public void Show(DialogueEvent dialogueEvent, float choiceTimeSeconds)
    {
        current = dialogueEvent;

        if (current == null)
        {
            Hide();
            return;
        }

        gameObject.SetActive(true);

        titleText.text = current.titel ?? "";
        descriptionText.text = current.description ?? "";

        var opts = current.options ?? Array.Empty<DialogueOption>();

        SetupOptionButton(option1Button, option1Label, opts.Length > 0 ? opts[0] : null);
        SetupOptionButton(option2Button, option2Label, opts.Length > 1 ? opts[1] : null);

        StartTimer(choiceTimeSeconds);
    }

    public void Hide()
    {
        StopTimer();

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        titleText.text = "";
        descriptionText.text = "";

        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        current = null;
        gameObject.SetActive(false);
    }

    private void SetupOptionButton(Button button, TMP_Text label, DialogueOption option)
    {
        button.onClick.RemoveAllListeners();

        if (option == null)
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);
        if (label != null)
            label.text = option.description ?? "";

        button.onClick.AddListener(() =>
        {
            StopTimer();
            ApplyOptionEffects(option);
            OnOptionSelected?.Invoke(option);
            Hide();
        });
    }

    private void ApplyOptionEffects(DialogueOption option)
    {
        if (option == null || relationshipManager == null)
            return;

        if (option.budget != 0 && playerWallet != null)
        {
            Debug.Log($"Applying budget change: {option.budget}");
            if (option.budget > 0)
                playerWallet.Add(option.budget);
            else
                playerWallet.Spend(option.budget);
        }
        relationshipManager.ApplyDelta(option.partij1, option.waarde1);
        relationshipManager.ApplyDelta(option.partij2, option.waarde2);
    }

    private void StartTimer(float seconds)
    {
        if (timerFillImage == null)
            return;

        if (seconds <= 0f)
        {
            timerRunning = false;
            if (hideBarWhenNoTimer)
                timerFillImage.gameObject.SetActive(false);
            return;
        }

        timerFillImage.gameObject.SetActive(true);

        timerDuration = seconds;
        timerRemaining = seconds;
        timerRunning = true;

        SetBar(1f);
    }

    private void StopTimer()
    {
        timerRunning = false;
        if (timerFillImage != null)
        {
            SetBar(1f);
            if (hideBarWhenNoTimer)
                timerFillImage.gameObject.SetActive(false);
        }
    }

    private void SetBar(float normalized)
    {
        if (timerFillImage != null)
            timerFillImage.fillAmount = normalized;
    }

    private void HandleTimeout()
    {
        if (
            autoPickFirstOptionOnTimeout
            && current != null
            && current.options != null
            && current.options.Length > 0
        )
        {
            var opt = current.options[0];
            ApplyOptionEffects(opt);
            OnOptionSelected?.Invoke(opt);
            Hide();
            return;
        }

        OnTimedOut?.Invoke();
        Hide();
    }
}
