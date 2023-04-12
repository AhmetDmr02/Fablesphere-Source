using UnityEngine.UI;
using UnityEngine;

public class ScriptPotion : MonoBehaviour
{
    public float Timee;
    public Text TimeText;
    public int Id;
    public bool started;
    private bool done;
    private SlotStatCalculator SSC;
    [SerializeField] IScriptPotion ourInterface;

    void Awake()
    {
        PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer.Add(this.gameObject);
    }
    public void Update()
    {
        if (!started) return;
        if (ourInterface == null)
        {
            ourInterface = this.gameObject.GetComponent<IScriptPotion>();
        }
        if (SSC == null)
        {
            SSC = PostProcessingManager.instance.gameObject.GetComponent<SlotStatCalculator>();
        }
        int var = (int)Timee;
        TimeText.text = var.ToString();
        if (Timee >= 0)
            Timee -= 1 * Time.deltaTime;
        if (!done)
        {
             ourInterface.doPotionStuff();   
        }
        if (Timee < 0 && !done && started)
        {
            SSC.ReadWithInvokeOf(1);
            ourInterface.removePotionBuffs();
            done = true;
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        PostProcessingManager.instance.gameObject.GetComponent<DrinkableStatPotionManager>().PotionBufferContainer.Remove(this.gameObject);
    }
}
