using UnityEngine;
using EZCameraShake;

public class EndingSceneMan : MonoBehaviour
{
    public static EndingSceneMan instance;
    public GameObject endingTrigger, endSfx1,endSfx2,endSfx3;
    public bool isEnded;
       
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
    public void endingSeq()
    {
        if (isEnded) return;
        isEnded = true;
        endSfx1.GetComponent<AudioSource>().Play();
        Invoke(nameof(play1), 0.1f);
        Invoke(nameof(play2), 3f);
    }
    public void play1()
    {
        endSfx2.GetComponent<AudioSource>().Play();
    }
    public void play2()
    {
        endSfx3.GetComponent<AudioSource>().Play();
        Invoke(nameof(openPortal), 1f);
    }
    public void openPortal()
    {
        CameraShaker.Instance.ShakeOnce(2, 1, 0.1f, 0.1f);
        endingTrigger.SetActive(true);
    }
}
