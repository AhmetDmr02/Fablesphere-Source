using UnityEngine.UI;
using UnityEngine;

public class SectorTemp : MonoBehaviour
{
    public int slotIndex;
    public int id;
    public int quantity;

    public Text rarityText, ItemnameText, QuantityText;
    public SectorButtons sacBut;
    public Image img;

    private void Start()
    {
        sacBut.ST = this;
    }
    public void calculate()
    {
        foreach (DataItem di in ActivatorManager.instance.GetComponent<Inventory>().DataItems)
        {
            if (id == di.Id)
            {
                rarityText.text = di.Rarity.ToString();
                ItemnameText.text = di.ItemName;
                QuantityText.text = quantity.ToString();
                img.sprite = di.Photo;
            }
        }
    }
}
