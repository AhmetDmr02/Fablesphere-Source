using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SectorButtons : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{
    public bool sacrificeMode;
    public SectorTemp ST;

    [ShowWhen("sacrificeMode",false)]
    public SacrificeMonolith SM;
    [ShowWhen("sacrificeMode", false)]
    [SerializeField] string Role;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Role == "Quit")
        {
            SM.QuitPanel();
        }
        if (Role == "thatisit")
        {
            List<Slot> pureSlot = Inventory.instance.AllSlots.ToList();
            foreach (Slot s in SM.slots)
            {
                pureSlot.Remove(s);
            }
            foreach (Slot slot in pureSlot)
            {
                slot.CleanSlot();
            }
            UISfxManager.instance.PlayWearSfx();
            UISfxManager.instance.gameObject.GetComponent<StatData>().CheckStatsAgainBecauseOfPotions();
            SM.setSacrificeDone();
            SM.QuitPanel();
        }
        if (sacrificeMode)
        {
            if (SM.HowMuchLeftTemp >= 1)
            {
                SM.HowMuchLeftTemp -= 1;
                SM.slots.Add(ActivatorManager.instance.GetComponent<Inventory>().AllSlots[ST.slotIndex]);
                SM.createdItems.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                UISfxManager.instance.PlayErrorSfx();
            }
        }
        UISfxManager.instance.PlayButtonClick();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISfxManager.instance.PlayButtonEnter();
    }
}
