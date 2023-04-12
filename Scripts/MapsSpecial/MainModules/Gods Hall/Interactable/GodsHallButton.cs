using EZCameraShake;
using UnityEngine;

public class GodsHallButton : MonoBehaviour,IInteractable
{
    [SerializeField] private VegvisirPlatform _platform1, _platform2, _platform3;
    [SerializeField] private AudioSource _clickSound,PortalOpen;
    [SerializeField] private ParticleSystem portalPS;
    [SerializeField] private Animator _buttonAnimator;

    public string GetLookAtDescription()
    {
        return "[F] To Click (It May Hurt)";
    }

    public Color GetTextColor()
    {
        return Color.blue;
    }

    public void OnInteract()
    {
        _clickSound.Play();
        _buttonAnimator.Play("ButtonPress");
        GodsHallMain mainHall = GodsHallMain.instance;
        if (mainHall.VegvisCompleted) return;
        if (mainHall.firstPlatform == _platform1.currentPlatform && mainHall.secondPlatform == _platform2.currentPlatform && mainHall.thirdPlatform == _platform3.currentPlatform)
        {
            PortalOpen.Play();
            portalPS.Play();
            mainHall.openVegvis();
        }
        else
        {
            PostProcessingManager.instance.gameObject.GetComponent<BarManager>().HP -= 100;
            UISfxManager.instance.PlayClothHitSound();
            EffectManager ES = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
            ES.CreateBlood(FPSController.instance.gameObject.GetComponent<Transform>());
            CameraShaker.Instance.ShakeOnce(1, 1 * 1.5f, 0f, 1f);
        }
    }
}
