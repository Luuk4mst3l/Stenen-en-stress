using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField]
    private int startingMoney = 0;

    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        Money = startingMoney;
        OnMoneyChanged?.Invoke(Money);
    }

    public void Add(int amount)
    {
        if (amount <= 0)
            return;
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool Spend(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }
}
