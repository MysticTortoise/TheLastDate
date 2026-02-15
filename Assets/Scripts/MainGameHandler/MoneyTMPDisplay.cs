using UnityEngine;
using TMPro;

public class MoneyTMPDisplay : MonoBehaviour
{
    public PlayerGlobalHandler playerGlobal;
    public TMP_Text moneyText;

    private float _lastMoney = float.MinValue;

    private void Awake()
    {
        if (playerGlobal == null)
            playerGlobal = PlayerGlobalHandler.GlobalHandler;

        if (playerGlobal == null)
            playerGlobal = FindAnyObjectByType<PlayerGlobalHandler>();

        if (moneyText == null)
            moneyText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (playerGlobal == null || playerGlobal.stats == null)
            return;

        float currentMoney = playerGlobal.stats.money;

        if (Mathf.Approximately(currentMoney, _lastMoney))
            return;

        _lastMoney = currentMoney;

        moneyText.text = Mathf.RoundToInt(currentMoney).ToString("N0");
    }
}
