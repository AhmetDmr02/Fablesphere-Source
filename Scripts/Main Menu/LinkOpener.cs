using UnityEngine.EventSystems;
using UnityEngine;

public class LinkOpener : MonoBehaviour, IPointerClickHandler
{
    public string urlToOpen;
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(urlToOpen);
    }
}
