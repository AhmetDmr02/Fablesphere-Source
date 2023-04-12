using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStatTrigger : MonoBehaviour
{
    private Slot slot;
    private int PreId;
    public bool PlaySfx;
    void Update()
    {
        if (slot == null)
        {
            slot = gameObject.GetComponent<Slot>();
        }
        if (slot.Id == 0)
        {
            if (PreId == 0)
            {
                return;
            }
            else
            {
                PreId = 0;
                SlotStatCalculator SSC = PostProcessingManager.instance.gameObject.GetComponent<SlotStatCalculator>();
                SSC.ReadStats();
                if (PlaySfx)
                {
                    UISfxManager Sfx = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
                    Sfx.PlayWearSfx();
                }
            }
        }
        if (slot.Id != 0)
        {
            if (slot.Id != PreId)
            {
                PreId = slot.Id;
                SlotStatCalculator SSC = PostProcessingManager.instance.gameObject.GetComponent<SlotStatCalculator>();
                SSC.ReadStats();
                if (PlaySfx)
                {
                    UISfxManager Sfx = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
                    Sfx.PlayWearSfx();
                }
            }
        }
    }
}
