using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicModuleInformations : MonoBehaviour
{
    [SerializeField] private PostprocessingData _postProcessingSetting;
    [SerializeField] private AudioReverbPreset audioPreset;
    private PostProcessingManager _postManager;
    [SerializeField] private bool _customFootsteps;
    public Transform playerStartPos;
    public Transform playerStartPosReverse;
    [SerializeField] [ShowWhen("_customFootsteps",true)] private footStepSpecs footstepSpecs;
    private bool _isFootstepConfigured;

    void Start()
    {
        _postManager = PostProcessingManager.instance;
    }

    void Update()
    {
        if (!isActiveAndEnabled) return;
        if (_postManager == null) _postManager = PostProcessingManager.instance;
        if (LoadingManager.instance.StartPos_ == null) LoadingManager.instance.StartPos_ = playerStartPos;
        if (LoadingManager.instance.StartPosReverse_ == null) LoadingManager.instance.StartPosReverse_ = playerStartPosReverse;
        if (_postManager.getActiveModuleObject() != this.gameObject)
        {
            _postManager.setActiveModuleObject(this.gameObject);
            PostProcessingManager.instance.handlePostProcessingSettings(_postProcessingSetting);
        }
        if (!_postManager.isPoisoned && ModuleSoundManager.instance.currentSoundKind != audioPreset)
        {
            ModuleSoundManager.instance.setSoundEffect(audioPreset);
            PostProcessingManager.instance.handlePostProcessingSettings(_postProcessingSetting);
        }
        if (!_postManager.isPoisoned && PostProcessingManager.instance.poisonHandled) PostProcessingManager.instance.poisonHandled = false;
        if (_postManager.isPoisoned) ModuleSoundManager.instance.setSoundEffect(AudioReverbPreset.Drugged);
        if (_postManager.isPoisoned && !_postManager.poisonHandled)
        {
            PostProcessingManager.instance.handlePostProcessingSettings(_postProcessingSetting);
            PostProcessingManager.instance.poisonHandled = true;
        }
        if (_customFootsteps && !_isFootstepConfigured)
        {
            _isFootstepConfigured = true;
            _postManager.HandleFPSControllerFootstepSounds(footstepSpecs);
        }else if(!_customFootsteps && !_isFootstepConfigured)
        {
            _isFootstepConfigured = true;
            _postManager.DontHandleFPSControllerFootstepSounds();
        }
    }
    public PostprocessingData getProcessSettings()
    {
        return _postProcessingSetting;
    }
}
[System.Serializable]
public class footStepSpecs
{
    public AudioClip[] clips;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public float footstepDelay;
}

