using System.Collections;
using UnityEngine;

public class StakeHolders : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip signalClip;

    [SerializeField]
    private RectTransform target;

    [SerializeField]
    private float scaleMultiplier = 1.15f;

    [SerializeField]
    private float halfCycleSeconds = 0.5f;

    [SerializeField]
    private int pulses = 5;

    [SerializeField]
    private bool ignoreTimeScale = false;

    [SerializeField]
    private DialogueRunner dialogueRunner;

    public bool IsNotified { get; private set; } = false;
    private bool HasBeenClicked = false;
    private bool IsPulseActive = false;

    private Vector3 _baseScale;
    private Coroutine _running;

    private void Awake()
    {
        if (target == null)
            target = GetComponentInChildren<RectTransform>();
        _baseScale = target.localScale;
    }

    public void Signal()
    {
        if (IsPulseActive)
            return;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource != null && signalClip != null)
            audioSource.PlayOneShot(signalClip, 1f);

        IsNotified = true;
        HasBeenClicked = false;
        _baseScale = target.localScale;
        StopPulse();
        _running = StartCoroutine(PulseRoutine());
    }

    public void OnClicked()
    {
        if (!IsNotified || HasBeenClicked || !IsPulseActive)
            return;

        HasBeenClicked = true;
        StopPulse();
        dialogueRunner?.Activate();
    }

    public void StopPulse()
    {
        if (_running != null)
        {
            StopCoroutine(_running);
            _running = null;
        }

        if (target != null)
            target.localScale = _baseScale;

        IsPulseActive = false;
    }

    private IEnumerator PulseRoutine()
    {
        IsPulseActive = true;
        Vector3 big = _baseScale * scaleMultiplier;

        for (int i = 0; i < pulses; i++)
        {
            yield return LerpScale(_baseScale, big, halfCycleSeconds);
            yield return LerpScale(big, _baseScale, halfCycleSeconds);
        }
        dialogueRunner?.SkipDialogueIndex();
        target.localScale = _baseScale;
        IsPulseActive = false;
        _running = null;
    }

    private IEnumerator LerpScale(Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float eased = p * p * (3f - 2f * p);
            target.localScale = Vector3.LerpUnclamped(from, to, eased);
            yield return null;
        }

        target.localScale = to;
    }
}
