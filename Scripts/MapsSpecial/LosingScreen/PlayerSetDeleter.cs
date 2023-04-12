using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetDeleter : MonoBehaviour
{
    private void Start()
    {
        GameObject go = GameObject.Find("PlayerSet");
        if (go != null)
        {
            Destroy(go);
        }
        if (MainDatabaseManager.instance != null)
        {
            if (Application.loadedLevel == 11 || Application.loadedLevel == 12)
            {
                Destroy(MainDatabaseManager.instance.gameObject);
            }
        }
    }
    public void GoMenu()
    {
        Application.LoadLevel(0);
    }
}
