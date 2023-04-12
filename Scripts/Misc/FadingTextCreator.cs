using TMPro;
using System.Collections;
using UnityEngine;

public class FadingTextCreator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fadingText;
    [SerializeField] private TMP_FontAsset defaultFont;
    public static FadingTextCreator instance;
    private bool ascend,waitState;
    private float kickspeed;
    private Color changeColor;
  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        fadingText.enabled = false;
    }
    private void FixedUpdate()
    {
        if (ascend)
        {
            if (changeColor.a < 1)
            {
                changeColor.a += kickspeed * Time.deltaTime;
                fadingText.color = changeColor;
            }
            else
            {
                waitState = true;
                //Decent Time
            }
        }
        else
        {
            if (changeColor.a > 0)
            {
                changeColor.a -= kickspeed * Time.deltaTime;
                fadingText.color = changeColor;
            }
            else
            {
                fadingText.enabled = false;
            }
        }
    }

    public void CreateFadeText(string content, float kickSpeed, float duration, TMP_FontAsset font, Color color)
    {
        StopCoroutine(InitFadeText(content, kickSpeed, duration, font, color));
        StartCoroutine(InitFadeText(content, kickSpeed, duration, font, color));
    }
    public IEnumerator InitFadeText(string content,float kickSpeed,float duration,TMP_FontAsset font,Color color)
    {
        // kickSpeed = kickSpeed / 10;
        ascend = true;
        waitState = false;
        fadingText.enabled = true;
        fadingText.font = font == null ? fadingText.font = defaultFont : fadingText.font = font;
        changeColor = new Color(color.r, color.g, color.b, 0);
        fadingText.color = changeColor;
        fadingText.text = content;
        kickspeed = kickSpeed;
        yield return new WaitUntil(() => waitState == true);
        kickspeed = 0;
        yield return new WaitForSeconds(duration);
        kickspeed = kickSpeed;
        ascend = false;
        yield return null;
        StopCoroutine(InitFadeText(content, kickSpeed, duration, font, color));
    }
}
