using UnityEngine;

public class MoneyDebugButtons : MonoBehaviour
{
    [SerializeField] private PlayerWallet wallet;

    public void Add10()
    {
        wallet.Add(10);
    }

    public void Spend10()
    {
        bool ok = wallet.Spend(10);
        if (!ok) Debug.Log("Not enough money!");
    }
}
