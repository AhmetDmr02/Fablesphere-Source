using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public float MaxCooldown, Cooldown,CooldownRate;
    public bool OnCooldown;
    public GameObject CooldownObject;
    public Image CooldownBar;
    public PlaySwordSwing[] PSS;
    private void Update()
    {
        CooldownRate = Cooldown / MaxCooldown;
        CooldownBar.fillAmount = CooldownRate;
        if (OnCooldown)
        {
            CooldownObject.SetActive(true);
            if (Cooldown > 0)
            {
                Cooldown -= 1 * Time.deltaTime;
            }
            else
            {
                foreach (PlaySwordSwing PSSwings in PSS)
                {
                    PSSwings.Playing = false;
                    PSSwings.Playing2 = false;
                }
                Cooldown = 0;
                OnCooldown = false;
            }
        }
        else
        {
            CooldownObject.SetActive(false);
        }
    }
    public void CreateCooldown(float Cooldown_Script)
    {
        MaxCooldown = Cooldown_Script;
        Cooldown = MaxCooldown;
        OnCooldown = true;
    }
}
