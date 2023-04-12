using UnityEngine;

public class SpawnerMobAddon : MonoBehaviour,IAfterSpawnerMethods
{
    public Transform[] patrolLocations;

    public void editGO(GameObject GO)
    {
        if (GO.GetComponent<IMobSerializer>() == null) return;
        GO.GetComponent<IMobSerializer>().setPatrolLocation(patrolLocations);
    }
}
