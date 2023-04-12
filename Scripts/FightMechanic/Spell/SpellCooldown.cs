using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldown : MonoBehaviour
{
    public float MaxCooldown, Cooldown, CooldownRate;
    public bool OnCooldown;
    public Image CooldownImage;
    private void Start()
    {
        Cooldown = 0;
        MaxCooldown = 1;
    }
    private void Update()
    {
        CooldownRate = Cooldown / MaxCooldown;
        CooldownImage.fillAmount = CooldownRate;
        if (OnCooldown)
        {
            if (Cooldown > 0)
            {
                Cooldown -= 1 * Time.deltaTime;
            }
            else
            {
                Cooldown = 0;
                OnCooldown = false;
            }
        }
    }

    public void CreateCooldown(float Cooldown_)
    {
        MaxCooldown = Cooldown_;
        Cooldown = MaxCooldown;
        OnCooldown = true;
    }
}
