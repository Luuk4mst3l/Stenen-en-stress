using System;
using System.Collections.Generic;
using UnityEngine;

public class StakeholderRelationshipManager : MonoBehaviour
{
    [Serializable]
    public class StakeholderBinding
    {
        [Tooltip("Moet matchen met JSON 'partij' (bv. groene, investor, budget)")]
        public string partij;

        [Tooltip("UI bar/heart controller voor deze partij (niet nodig voor budget)")]
        public RelationshipTrackerUI relationshipUI;

        [Tooltip("Optioneel: laat stakeholder icoon pulsen bij verandering")]
        public StakeHolders pulse;

        [Tooltip("Startwaarde voor deze partij")]
        public float startingValue = 0f;
    }

    [Header("Wiring")]
    //[SerializeField] private PlayerWallet wallet;

    [Header("Stakeholder UI bindings")]
    [SerializeField] private StakeholderBinding[] bindings;

    private readonly Dictionary<string, StakeholderBinding> _bindingByKey = new Dictionary<string, StakeholderBinding>();
    private readonly Dictionary<string, float> _valueByKey = new Dictionary<string, float>();

    private void Awake()
    {
        //if (wallet == null) wallet = FindObjectOfType<PlayerWallet>();

        _bindingByKey.Clear();
        _valueByKey.Clear();

        if (bindings == null) return;

        for (int i = 0; i < bindings.Length; i++)
        {
            var b = bindings[i];
            if (b == null || string.IsNullOrWhiteSpace(b.partij)) continue;

            string key = NormalizeKey(b.partij);

            _bindingByKey[key] = b;
            _valueByKey[key] = b.startingValue;

            if (b.relationshipUI != null)
            {
                // Zorg dat testmode niet constant overschrijft
                b.relationshipUI.useTestValue = false;
                b.relationshipUI.SetRelationship(b.startingValue);
            }
        }
    }

    public void ApplyDelta(string partij, int delta)
    {
        if (string.IsNullOrWhiteSpace(partij) || delta == 0) return;

        string key = NormalizeKey(partij);


        if (key == "budget")
        {
            //ApplyMoney(delta);
            return;
        }

        float current = _valueByKey.TryGetValue(key, out var v) ? v : 0f;
        float next = current + delta;
        _valueByKey[key] = next;

        if (_bindingByKey.TryGetValue(key, out var binding))
        {
            if (binding.relationshipUI != null)
            {
                binding.relationshipUI.useTestValue = false;
                binding.relationshipUI.SetRelationship(next);
            }

            if (binding.pulse != null)
            {
                binding.pulse.Signal();
            }
        }
        else
        {
            Debug.LogWarning($"No stakeholder binding found for partij '{partij}'. (key '{key}')");
        }
    }

//     private void ApplyMoney(int delta)
//     {
//         if (wallet == null)
//         {
//             Debug.LogWarning("PlayerWallet not found; cannot apply budget delta.");
//             return;
//         }
//
//         if (delta > 0)
//         {
//             wallet.Add(delta);
//         }
//         else
//         {
//             bool ok = wallet.Spend(-delta);
//             if (!ok)
//             {
//                 Debug.LogWarning($"Not enough money to spend {-delta}. Budget unchanged.");
//             }
//         }
//     }

    private string NormalizeKey(string s) => s.Trim().ToLowerInvariant();
}
