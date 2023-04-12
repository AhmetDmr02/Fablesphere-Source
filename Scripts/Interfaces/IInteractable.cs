using UnityEngine;

public interface IInteractable
{
    void OnInteract();
    string GetLookAtDescription();
    Color GetTextColor();
}
