using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
            PostProcessingManager.instance.gameObject.GetComponent<BarManager>().Die();
    }
}
