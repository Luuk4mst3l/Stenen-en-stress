using TMPro;
using UnityEngine;

public class MoneyBadgeUI : MonoBehaviour
{
    [SerializeField]
    private PlayerWallet wallet;

    [SerializeField]
    private TMP_Text moneyText;

    private void OnEnable()
    {
        if (wallet != null)
            wallet.OnMoneyChanged += UpdateText;
    }

    private void OnDisable()
    {
        if (wallet != null)
            wallet.OnMoneyChanged -= UpdateText;
    }

    private void Start()
    {
        if (wallet != null)
            UpdateText(wallet.Money);
    }

    private void UpdateText(int amount)
    {
        if (moneyText != null)
            moneyText.text = amount.ToString();
    }
}
