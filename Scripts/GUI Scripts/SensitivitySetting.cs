using UnityEngine;
using UnityEngine.UI;
public class SensitivitySetting : MonoBehaviour
{
    public Slider sensSlider;
    public Canvas sensPanel;
    void Start()
    {
        sensSlider.onValueChanged.AddListener(val => setSens(val));
        float f = PlayerPrefs.HasKey("MouseSens") ? PlayerPrefs.GetFloat("MouseSens") : 140;
        sensSlider.SetValueWithoutNotify(f);
        if (PlayerPrefs.HasKey("MouseSens"))
        {
            setSens(f);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ActivatorManager.instance.CutsceneActive) return;
            if (ActivatorManager.instance.ActiveObject != this.gameObject && ActivatorManager.instance.SomeoneActive) return;
            if (ActivatorManager.instance.ActiveObject == this.gameObject)
            {
                ActivatorManager.instance.SomeoneActive = false;
                ActivatorManager.instance.ActiveObject = null;
                sensPanel.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                ActivatorManager.instance.SomeoneActive = true;
                ActivatorManager.instance.ActiveObject = this.gameObject;
                sensPanel.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    void setSens(float sensInt)
    {
        FPSController.instance.Camera.GetComponent<CameraLook>().Sensitivity = sensInt;
        PlayerPrefs.SetFloat("MouseSens", sensInt);
        PlayerPrefs.Save();
    }
}
