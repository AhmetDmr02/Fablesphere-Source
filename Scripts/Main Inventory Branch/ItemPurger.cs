using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemPurger : MonoBehaviour,IPointerClickHandler
{
    //Writed By Admr0 For Test -And Successfully Completed
    GoneManager GM;
    Item item;

    public void OnPointerClick(PointerEventData eventdata)
    {
        GM = PostProcessingManager.instance.gameObject.GetComponent<GoneManager>();
        if (eventdata.button == PointerEventData.InputButton.Left)
        {
            if (item == null)
                item = GameObject.FindGameObjectWithTag("Preview").GetComponent<Item>();
            if (item.Dragging)
            {
                if (item.Id == 49)//Gods prism
                    return;
                GM.InstantiateGone(item.Id, item.Quantity);
                item.CleanAllInfo();
            }

        }
        if (eventdata.button == PointerEventData.InputButton.Middle)
        {
            if (item == null)
                item = GameObject.FindGameObjectWithTag("Preview").GetComponent<Item>();
            if (item.Id == 49)
                return;
            if (item.Dragging && item.Quantity > 1)
            {
                GM.InstantiateGone(item.Id, 1);
                item.Quantity -= 1;
            }
            else if (item.Dragging && item.Quantity == 1)
            {
                GM.InstantiateGone(item.Id, 1);
                item.CleanAllInfo();
            }
        }
    }
}
