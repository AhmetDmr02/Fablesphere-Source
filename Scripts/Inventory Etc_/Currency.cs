using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    [SerializeField] private int tradeGem;
    public Text tradeGemText;
    public Text mercyText;
    public Text tradeMenuGemText;

    private void Start()
    {
        recalculateMercy();
    }
    void Update()
    {
        tradeGemText.text = tradeGem.ToString();
    }

    public void recalculateMercy()
    {
        mercyText.text = Inventory.instance.gameObject.GetComponent<BarManager>().maxMercy.ToString();
    }
    public void DecreaseGem(int decraseAmount)
    {
        tradeGem -= decraseAmount;
    }
    public void IncreaseGem(int IncreaseAmount)
    {
        tradeGem += IncreaseAmount;
    }
    public int GetGem()
    {
        return tradeGem;
    }
}
