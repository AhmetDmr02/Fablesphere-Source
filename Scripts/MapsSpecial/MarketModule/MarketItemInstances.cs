using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MarketItemInstances : MonoBehaviour,IPointerEnterHandler
{

    [Header("GUI Stuff")]
    [SerializeField] private Text RuneStone_Text;
    [SerializeField] private Text itemName_Text;
    [SerializeField] private Text itemCount_Text;
    [SerializeField] private Text Rarity_Text;
    [SerializeField] private Image itemPictureImage;
    [Header("Internal Info")]
    [SerializeField] private int Count;
    [SerializeField] private int Price;
    [SerializeField] private int ItemID;
    [SerializeField] private Sprite ItemPicture_;
    [SerializeField] private string Rarity_;
    [SerializeField] private Color rarityC;
    [SerializeField] private string ItemName_;
    public int COUNT => Count;
    public int PRICE => Price;
    public int ITEMID => ItemID;
    public Sprite ITEMPIC => ItemPicture_;
    public string RARITY => Rarity_;
    public string ITEMNAME => ItemName_;



    [HideInInspector] public UISfxManager USM;
    public TradeStoneMain TSM;



    public void ConstructCell(int ID,int SellCost,string ItemName,int ItemCount,string Rarity,Sprite ItemPicture)
    {
        ItemID = ID;
        Price = SellCost;
        Count = ItemCount;
        ItemPicture_ = ItemPicture;
        Rarity_ = Rarity;
        ItemName_ = ItemName;
        if (ItemCount == 0)
        {
            ItemCount = 1;
        }
        Rarity_Text.text = "Rarity: " + "\n" + Rarity;
        #region Color Assignment
        if (Rarity == "Legendary")
        {
            Rarity_Text.color = Color.yellow;
            itemName_Text.color = Color.yellow;
            itemCount_Text.color = Color.yellow;
            rarityC = Color.yellow;
        }
        if (Rarity == "Epic")
        {
            Rarity_Text.color = Color.magenta;
            itemName_Text.color = Color.magenta;
            itemCount_Text.color = Color.magenta;
            rarityC = Color.magenta;

        }
        if (Rarity == "Rare")
        {
            Rarity_Text.color = Color.blue;
            itemName_Text.color = Color.blue;
            itemCount_Text.color = Color.blue;
            rarityC = Color.blue;
        }
        if (Rarity == "Common")
        {
            Rarity_Text.color = Color.gray;
            itemName_Text.color = Color.gray;
            itemCount_Text.color = Color.gray;
            rarityC = Color.gray;
        }
        #endregion
        RuneStone_Text.text = SellCost.ToString();
        itemPictureImage.sprite = ItemPicture;
        itemName_Text.text = ItemName;
        itemCount_Text.text = "Stock: " + ItemCount.ToString();
    }
    public void Sell()
    {
        if (Count > 1)
        {
            TSM.SellRequest(this, false);
        }
        else
        {
            TSM.SellRequest(this, true);
        }
    }

    public void Buy()
    {
        if (Count > 1)
        {
            TSM.BuyRequest(this, false);
        }
        else
        {
            TSM.BuyRequest(this, true);
        }
    }

    public void DecreaseCount()
    {
        Count -= 1;
        itemCount_Text.text = "Stack: " + Count;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemID > 0 || TSM.nameItem != null)
        {
            TSM.nameItem.text = ItemName_;
            TSM.nameItem.color = rarityC;
            DataItem ourDI = new DataItem();
            foreach (DataItem di in Inventory.instance.DataItems)
            {
                if (di.Id == ItemID)
                {
                    ourDI = di;
                }
            }
            TSM.descItem.text = ourDI.ItemDescription;
            TSM.descItem.color = Color.gray;
            string statString = PostProcessingManager.instance.gameObject.GetComponent<StatData>().GetStats(ItemID, ourDI.Category);
            TSM.statItem.text = statString;
        }
    }

    #region Getting Variables
    public int GetID()
    {
        return ItemID;
    }
    public int GetCount()
    {
        return Count;
    }
    public int GetPrice()
    {
        return Price;
    }
    #endregion
}
