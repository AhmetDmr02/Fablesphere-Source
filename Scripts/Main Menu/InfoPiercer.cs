using UnityEngine;

public class InfoPiercer : MonoBehaviour
{
    public float audioFloat;
    public RandomRooms roomStats;
    public bool meSetVars;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        audioFloat = 1f;
    }
    private void FixedUpdate()
    {
        if (!meSetVars && Application.loadedLevel == 1)
        {
            meSetVars = true;
            AudioListener.volume = audioFloat;
            if (audioFloat == 0) AudioListener.pause = true;
            ProceduralModuleGenerator.instance.setVariablesByRoomStats(roomStats);
            EnteranceModuleScript.instance.reloadScript();
            LoadingManager.instance.desiredFloatVolume = audioFloat;
            LoadingManager.instance.infoPierced = true;
            Destroy(this.gameObject);
        }
    }
}
