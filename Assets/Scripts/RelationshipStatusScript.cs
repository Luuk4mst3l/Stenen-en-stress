using UnityEngine;

public class RelationshipTrackerUI : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField]
    private RectTransform leftPoint;

    [SerializeField]
    private RectTransform rightPoint;

    [SerializeField]
    private RectTransform heart;

    [Header("Value Range")]
    [SerializeField]
    private float minValue = -100f;

    [SerializeField]
    private float maxValue = 100f;

    [Header("Motion")]
    [SerializeField]
    private bool smoothMove = true;

    [SerializeField]
    private float smoothSpeed = 12f;

    [Header("Testing")]
    public bool useTestValue = true;

    [Range(-100f, 100f)]
    public float testRelationshipValue = 0f;

    private float targetValue;

    private void Awake()
    {
        targetValue = minValue;
        UpdateHeartInstant(targetValue);
    }

    private void Update()
    {
        if (useTestValue)
        {
            SetRelationship(testRelationshipValue);
        }

        if (!smoothMove)
            return;

        float current = GetCurrentValueFromHeart();
        float next = Mathf.Lerp(
            current,
            targetValue,
            1f - Mathf.Exp(-smoothSpeed * Time.unscaledDeltaTime)
        );
        UpdateHeartInstant(next);
    }

    public void SetRelationship(float value)
    {
        targetValue = Mathf.Clamp(value, minValue, maxValue);

        if (!smoothMove)
            UpdateHeartInstant(targetValue);
    }

    private void UpdateHeartInstant(float value)
    {
        float t = Mathf.InverseLerp(minValue, maxValue, value);
        heart.position = Vector3.Lerp(leftPoint.position, rightPoint.position, t);
    }

    private float GetCurrentValueFromHeart()
    {
        Vector3 a = leftPoint.position;
        Vector3 b = rightPoint.position;

        float lineLen = Vector3.Distance(a, b);
        if (lineLen <= 0.0001f)
            return targetValue;

        float heartLen = Vector3.Distance(a, heart.position);
        float t = Mathf.Clamp01(heartLen / lineLen);

        return Mathf.Lerp(minValue, maxValue, t);
    }
}
