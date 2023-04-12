using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ActivatorManager.instance.SomeoneActive = true;
        ActivatorManager.instance.ActiveObject = this.gameObject;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EffectManager.instance.CreateBlackoutEffect(1, 10, false, 1);
        Invoke("slas", 1f);
    }
    public void slas()
    {
        Application.LoadLevel("Scenes/Winning");
    }
}
