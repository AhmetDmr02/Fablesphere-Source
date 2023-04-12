using UnityEngine.EventSystems;
using UnityEngine;

public class MarketCanvasButtons : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{
    public TradeStoneMain TSM;
    [SerializeField] string Role;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Role == "Quit")
        {
            TSM.QuitTradePanel();
        }
        if (Role == "SwitchSell")
        {
            TSM.RecalculateSellStuff();
            TSM.SwitchToSelling();
        }
        if (Role == "SwitchBuy")
        {
            TSM.SwitchToBuying();
        }
        TSM.USM.PlayButtonClick();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TSM.USM.PlayButtonEnter();
    }
}
