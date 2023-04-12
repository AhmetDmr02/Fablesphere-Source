using UnityEngine;
using TMPro;

public class MonolithInfoGiver : MonoBehaviour, IInteractable
{
    [SerializeField] private float _duration, _kickSpeed;
    [SerializeField] private string _monolithName;
    [SerializeField] [TextArea] private string _description;
    [SerializeField] private TMP_FontAsset encrypted;
    [SerializeField] private bool isItMAIN;

    public string GetLookAtDescription()
    {
        return "[F] To See Monolith Explanation";
    }

    public Color GetTextColor()
    {
        return Color.cyan;
    }

    public void OnInteract()
    {
        string ourContent = "";
        if (isItMAIN)
        {
            ourContent = _monolithName + ": \n" + _description + "\n" + "Chieves Whispers You This: \n" + GodsHallMain.instance.firstPlatform + GodsHallMain.instance.secondPlatform + GodsHallMain.instance.thirdPlatform;
        }
        else
        {
            ourContent = _monolithName + ": \n" + _description;
        }
        if (GodsHallMain.instance.isPhilterOfRealityActive())
        {
            FadingTextCreator.instance.CreateFadeText(ourContent, _kickSpeed, _duration, null, Color.cyan);
        }
        else
        {
            FadingTextCreator.instance.CreateFadeText(ourContent, _kickSpeed, _duration, encrypted, Color.cyan);
        }
    }
}
