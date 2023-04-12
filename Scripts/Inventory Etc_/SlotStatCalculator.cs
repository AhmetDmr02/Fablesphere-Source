using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotStatCalculator : MonoBehaviour
{
    private StatData data_Reader;
    [SerializeField] private GameObject[] SetSwordSpeedVar;
    public Text Stat_Text;
    public Slot[] StatSlots;

    public void ReadStats()
    {
        data_Reader = gameObject.GetComponent<StatData>();
        string TextString = data_Reader.GetOverAllStatsFromSlots(StatSlots);
        Stat_Text.text = TextString;
        SetSwordsSpeed();
    }
    public void ReadWithInvokeOf(float seconds)
    {
        Invoke("ReadStats", seconds);
    }

    //Setting Sword Hit Delay Variable Of Speed
    private void SetSwordsSpeed()
    {
        data_Reader = gameObject.GetComponent<StatData>();
        float[] Stat = data_Reader.GetPureStatArrayFromSlots(StatSlots);
        foreach (GameObject GO in SetSwordSpeedVar)
        {
            GO.GetComponent<PlaySwordSwing>().setSpeedStat(Stat[3]);
            GO.GetComponent<PlaySwordSwing>().CalculateVariables();
        }
    }
}
