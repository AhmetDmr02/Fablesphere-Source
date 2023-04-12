using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPotion : MonoBehaviour
{
    public float Timee;
    public Text TimeText;
    public int Str,PhysicalDef,MagicalDef,Speed;
    public int Id;
    public bool started;
    private bool done;

    private SlotStatCalculator SSC;

    void Awake()
    {
        PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer.Add(this.gameObject);
    }
    public void Update()
    {
        if (SSC == null)
        {
            SSC = PostProcessingManager.instance.gameObject.GetComponent<SlotStatCalculator>();
        }
        int var = (int)Timee;
        TimeText.text = var.ToString(); 
        if (Timee > 0)
            Timee -= 1 * Time.deltaTime;
        if (Timee < 0 && !done && started)
        {
            SSC.ReadWithInvokeOf(1);
            done = true;
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer.Remove(this.gameObject);
    }
}
