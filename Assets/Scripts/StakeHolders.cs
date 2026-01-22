using System.Collections;
using UnityEngine;

public class StakeHolders : MonoBehaviour
{
    [Header("What to pulse")]
    [SerializeField]
    private RectTransform target;

    [Header("Pulse settings")]
    [SerializeField]
    private float scaleMultiplier = 1.15f;

    [SerializeField]
    private float halfCycleSeconds = 0.5f;

    [SerializeField]
    private int pulses = 20;

    [SerializeField]
    private bool ignoreTimeScale = false;

    private Vector3 _baseScale;
    private Coroutine _running;

    private void Awake()
    {
        if (target == null)
            target = GetComponentInChildren<RectTransform>();
        _baseScale = target.localScale;
    }

    private void OnDisable()
    {
        StopPulse();
        if (target != null)
            target.localScale = _baseScale;
    }

    public void Signal()
    {
        StartPulse();
    }

    public void StartPulse()
    {
        if (target == null)
            return;

        _baseScale = target.localScale;
        StopPulse();
        _running = StartCoroutine(PulseRoutine());
    }

    public void StopPulse()
    {
        if (_running != null)
        {
            StopCoroutine(_running);
            _running = null;
        }
    }

    private IEnumerator PulseRoutine()
    {
        Vector3 big = _baseScale * scaleMultiplier;

        for (int i = 0; i < pulses; i++)
        {
            yield return LerpScale(_baseScale, big, halfCycleSeconds);
            yield return LerpScale(big, _baseScale, halfCycleSeconds);
        }

        target.localScale = _baseScale;
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
