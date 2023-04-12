using UnityEngine.EventSystems;
using UnityEngine;

public class MarketItemButtons : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Set Modes")]
    [SerializeField] private bool SellMode;
    MarketItemInstances myInstance;

    public void Awake()
    {
        myInstance = GetComponentInParent<MarketItemInstances>();
    }
    public void Start()
    {
        myInstance = GetComponentInParent<MarketItemInstances>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SellMode)
        {
            myInstance.USM.PlayButtonClick();
            myInstance.Sell();
        }
        else
        {
            int Currency = myInstance.TSM.C.GetGem();
            if (myInstance.GetPrice() > Currency)
            {
                myInstance.USM.PlayErrorSfx();
            }
            else
            {
                myInstance.TSM.inv.CalculateSlotCanHandle(myInstance.GetID());
                if (myInstance.TSM.inv.AllSlotsCanHandleForLastAddedItem < 1)
                {
                    myInstance.USM.PlayErrorSfx();
                }else
                {
                    myInstance.USM.PlayButtonClick();
                    myInstance.Buy();
                }
            }

           
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myInstance.USM.PlayButtonEnter();
    }
}
