using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AvailableId : MonoBehaviour
{
    [Colored(1, 0, 1)]
    public int NextAvailableId;
    Inventory inv;
    void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
    }


    void Update()
    {
        CalculateAvailableId();
    }
    public void CalculateAvailableId()
    {
        NextAvailableId = inv.DataItems.Length;
        NextAvailableId += 1;
    }
}
