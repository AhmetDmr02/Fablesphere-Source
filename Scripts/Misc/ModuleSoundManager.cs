using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSoundManager : MonoBehaviour
{
    public static ModuleSoundManager instance;
    public AudioReverbPreset currentSoundKind;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    public void setSoundEffect(AudioReverbPreset audioPreset)
    {
        if (currentSoundKind == audioPreset) return;
        currentSoundKind = audioPreset;
        FPSController.PlayerObject.GetComponent<AudioReverbZone>().reverbPreset = audioPreset;
    }
}
