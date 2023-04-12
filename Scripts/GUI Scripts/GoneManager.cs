using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoneManager : MonoBehaviour
{
    public GameObject ParentObject;
    public GameObject GonePopUp;
    private Text GonePopUpText, GonePopUpNameText;
    private Image GonePopUpImage;
    Inventory inv;

    private void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
    }

    public void InstantiateGone(int itemId, int Count)
    {
        GameObject GO = Instantiate(GonePopUp, ParentObject.transform);
        GonePopUpNameText = GO.transform.GetChild(1).GetComponent<Text>();
        GonePopUpImage = GO.transform.GetChild(0).GetComponent<Image>();
        GonePopUpText = GO.transform.GetChild(2).GetComponent<Text>();
        for (int i = 0; i < inv.DataItems.Length; i++)
        {
            if (inv.DataItems[i].Id == itemId)
            {
                GonePopUpNameText.text = inv.DataItems[i].ItemName;
                GonePopUpText.text = "-" + Count;
                GonePopUpImage.sprite = inv.DataItems[i].Photo;
            }
        }
    }
}
