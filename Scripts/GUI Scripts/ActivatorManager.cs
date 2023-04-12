using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorManager : MonoBehaviour
{
    public Canvas InventoryCanvas;
    [HideInInspector]
    public bool SomeoneActive;
    public bool CutsceneActive;
    public bool ChestPoolActive;
    public GameObject ActiveObject,ChestPanel;
    public MonoBehaviour[] DisableObject;//Disable When Something Opens
    public GunSway[] DisableSways; //Disable Sword Sway Animations
    public static ActivatorManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        InventoryCanvas.enabled = false;
    }

    void Update()
    {
        if (CutsceneActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (MonoBehaviour Behaviors in DisableObject)
            {
                Behaviors.enabled = false;
            }
            foreach (GunSway swayyers in DisableSways)
            {
                swayyers.enabled = false;
            }
        }
        else if (!SomeoneActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (MonoBehaviour Behaviors in DisableObject)
            {
                Behaviors.enabled = true;
            }
            foreach (GunSway swayyers in DisableSways)
            {
                swayyers.enabled = true;
            }
        }
        if (SomeoneActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            foreach (MonoBehaviour Behaviors in DisableObject)
            {
                Behaviors.enabled = false;
            }
            foreach (GunSway swayyers in DisableSways)
            {
                swayyers.enabled = false;
            }
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (MonoBehaviour Behaviors in DisableObject)
            {
                Behaviors.enabled = true;
            }
            foreach (GunSway swayyers in DisableSways)
            {
                swayyers.enabled = true;
            }
        }
        if (ChestPoolActive)
        {
            ChestPanel.SetActive(true);
        }
        else
        {
            ChestPanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!CutsceneActive)
            {
                if (SomeoneActive)
                {
                    if (ActiveObject == InventoryCanvas.gameObject)
                    {
                        InventoryCanvas.enabled = !InventoryCanvas.enabled;
                        if (InventoryCanvas.enabled == false)
                        {
                            SomeoneActive = false;
                            ActiveObject = null;
                            ChestPoolActive = false;
                            if (ChestPoolActive)
                            {
                                ChestPanel.SetActive(true);
                            }
                            else
                            {
                                ChestPanel.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    InventoryCanvas.enabled = !InventoryCanvas.enabled;
                    SomeoneActive = true;
                    ActiveObject = InventoryCanvas.gameObject;
                }
            }
        }
    }
    public void OpenInventory()
    {
        if (SomeoneActive)
        {
            if (ActiveObject == InventoryCanvas.gameObject)
            {
                InventoryCanvas.enabled = !InventoryCanvas.enabled;
                if (InventoryCanvas.enabled == false)
                {
                    SomeoneActive = false;
                    ActiveObject = null;
                    ChestPoolActive = false;
                    if (ChestPoolActive)
                    {
                        ChestPanel.SetActive(true);
                    }
                    else
                    {
                        ChestPanel.SetActive(false);
                    }
                }
            }
        }
        else
        {
            InventoryCanvas.enabled = !InventoryCanvas.enabled;
            SomeoneActive = true;
            ActiveObject = InventoryCanvas.gameObject;
        }
    }
}
