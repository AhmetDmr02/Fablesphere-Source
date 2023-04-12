using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemToStat : MonoBehaviour,IPointerEnterHandler
{
    private StatData statData;
    public Text statText;
    public void OnPointerEnter(PointerEventData event_data)
    {
        if (this.gameObject.GetComponent<Slot>().Category == "Weapon" || this.gameObject.GetComponent<Slot>().Category == "Armour" || this.gameObject.GetComponent<Slot>().Category == "Helmet")
        {
            statData = PostProcessingManager.instance.gameObject.GetComponent<StatData>();
            string TextString = statData.GetStats(this.gameObject.GetComponent<Slot>().Id, this.gameObject.GetComponent<Slot>().Category);
            statText.text = TextString;
            Color TextColor = statData.GetColor(this.gameObject.GetComponent<Slot>().Id, this.gameObject.GetComponent<Slot>().Category);
            statText.color = TextColor;
        }
        else
        {
            statText.text = "";
        }
    }
}
