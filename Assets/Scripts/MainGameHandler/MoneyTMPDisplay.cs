using UnityEngine;
using TMPro;

public class MoneyTMPDisplay : MonoBehaviour
{
    public TMP_Text moneyText;

    private float _lastMoney = float.MinValue;

    private void Awake()
    {
        if (moneyText == null)
            moneyText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (PlayerGlobalHandler.GlobalHandler == null ||
            PlayerGlobalHandler.GlobalHandler.stats == null)
            return;

        float currentMoney = PlayerGlobalHandler.GlobalHandler.stats.money;

        if (Mathf.Approximately(currentMoney, _lastMoney))
            return;

        _lastMoney = currentMoney;

        moneyText.text = Mathf.RoundToInt(currentMoney).ToString("N0");
    }
}
