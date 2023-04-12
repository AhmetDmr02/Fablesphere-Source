using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketCanvas : MonoBehaviour
{
    [SerializeField] GameObject QuitButton, SellModeButton, BuyModeButton;
    [SerializeField] TextMeshProUGUI nameg, descg, statg;
    public TradeStoneMain TSM;

    private void Update()
    {
        if (QuitButton.GetComponent<MarketCanvasButtons>().TSM == null)
        {
            QuitButton.GetComponent<MarketCanvasButtons>().TSM = TSM;
        }
        if (SellModeButton.GetComponent<MarketCanvasButtons>().TSM == null)
        {
            SellModeButton.GetComponent<MarketCanvasButtons>().TSM = TSM;
        }
        if (BuyModeButton.GetComponent<MarketCanvasButtons>().TSM == null)
        {
            BuyModeButton.GetComponent<MarketCanvasButtons>().TSM = TSM;
        }
        if (TSM.nameItem == null)
        {
            TSM.nameItem = nameg;
            TSM.descItem = descg;
            TSM.statItem = statg;
        }
    }
}
