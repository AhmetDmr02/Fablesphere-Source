using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTravellerBooksInfo : MonoBehaviour, IInteractable
{
    public string GetLookAtDescription()
    {
        return "Press [F] To Get More Info";
    }

    public Color GetTextColor()
    {
        return Color.blue;
    }

    public void OnInteract()
    {
        FadingTextCreator.instance.CreateFadeText("This books written by some stellar mage it contains some info but of course you cannot read it", 1f, 5f, null, Color.blue);
    }
}
