using UnityEngine;
using TMPro;

public class LateFadeText : MonoBehaviour
{
    private Color changeColor;
    [SerializeField] private TextMeshProUGUI mtext;
    [SerializeField] private float speed;
    [SerializeField] private float delay;
    private bool openUp;
    private void Start()
    {
        mtext = this.GetComponent<TextMeshProUGUI>();
        changeColor = mtext.color;
        changeColor = new Color(changeColor.r, changeColor.g, changeColor.b, 0);
        mtext.color = changeColor;
        Invoke("openBoolUp", delay);
    }
    private void Update()
    {
        if (!openUp) return;
        if (changeColor.a < 1)
        {
            changeColor.a += speed * Time.deltaTime;
            mtext.color = changeColor;
        }
    }
    private void openBoolUp()
    {
        openUp = true;
    }
}
