using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class WriteTextForItemIds : MonoBehaviour
{
    public int itemId;
    Inventory inventory;
    public int RemainIds;
    public bool Clicked;
    [SerializeField]
    private bool AddedBefore,Creating;
    void Start()
    {
        itemId = 1;
        inventory = gameObject.GetComponent<Inventory>();
        RemainIds = inventory.DataItems.Length;
    }
    void Update()
    {
        if (RemainIds > 0 && Clicked == true && !Creating)
        {
            CreateList();
        }
        else if (RemainIds <= 0 && Clicked)
        {
            string path = Application.dataPath + "/IdList.txt";
            Debug.Log(path);
            Clicked = false;
            Creating = false;
            AddedBefore = true;
        }
    }
    public void CreateList()
    {
        if (RemainIds == 0)
        {
            RemainIds = inventory.DataItems.Length;
            itemId = 1;
        }
        Creating = true;
        string WriteThis;
        string path = Application.dataPath + "/IdList.txt";
        Clicked = true;
        if (AddedBefore)
        {
            System.IO.File.WriteAllText(path, string.Empty);
            Debug.Log("Cleared All Old Data");
            AddedBefore = false;
        }
        if (RemainIds > 0)
        {
            for (int i = 0; i < inventory.DataItems.Length; i++)
            {
                if (inventory.DataItems[i].Id == itemId)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    WriteThis = "ItemId:" + itemId + " " + "|" + "ItemName:" + inventory.DataItems[i].ItemName + " " + "| " + "ItemCategory:" + inventory.DataItems[i].Category;
                    writer.WriteLine(WriteThis);
                    itemId += 1;
                    RemainIds -= 1;
                    Debug.Log(RemainIds);
                    writer.Close();
                    Creating = false;
                    break;
                }
            }
        }
    }
}
