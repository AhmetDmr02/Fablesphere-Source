using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager instance;
    private GameObject activeModuleObject;
    [SerializeField] private PostProcessVolume _postProcess;
    private AmbientOcclusion _ambientOcclusion;
    private ColorGrading _colorGrading;
    private Vignette _vignette;
    private Bloom _bloom;
    private LensDistortion _LensDistortion;
    private AutoExposure _exposure;
    [SerializeField]private PostprocessingData poisonedData;
    public bool isPoisoned,poisonHandled,hasPoisonProtection;
    public bool CritPotionActive;
    [SerializeField] private float decreaseFlash;

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
        _postProcess.profile.TryGetSettings(out _ambientOcclusion);
        _postProcess.profile.TryGetSettings(out _colorGrading);
        _postProcess.profile.TryGetSettings(out _vignette);
        _postProcess.profile.TryGetSettings(out _bloom);
        _postProcess.profile.TryGetSettings(out _LensDistortion);
        _postProcess.profile.TryGetSettings(out _exposure);
    }
    private void Update()
    {
        if (_vignette.intensity.value > 0)
        {
            _vignette.intensity.value -= decreaseFlash * Time.deltaTime;
        }
    }
    public void handlePostProcessingSettings(PostprocessingData _postProcessSetting)
    {
       if (isPoisoned)
       {
            #region Ambient Occlusion
            _ambientOcclusion.intensity.value = poisonedData._ambientIntensity;
            _ambientOcclusion.thicknessModifier.value = poisonedData._ambientThicknessModifier;
            _ambientOcclusion.color.value = poisonedData._ambientColor;
            _ambientOcclusion.ambientOnly.value = poisonedData._ambientOnly;
            #endregion
            #region Bloom
            _bloom.intensity.value = poisonedData._bloomIntensity;
            _bloom.diffusion.value = poisonedData._bloomDiffusion;
            #endregion
            #region Auto Exposure
            //This place is not necessary for now maybe we will add later
            #endregion
            #region Color Grading
            _colorGrading.hueShift.value = poisonedData._colorHueShift;
            _colorGrading.saturation.value = poisonedData._colorSaturation;
            _colorGrading.brightness.value = poisonedData._colorBrightness;
            _colorGrading.contrast.value = poisonedData._colorContrast;
            _colorGrading.mixerRedOutRedIn.value = poisonedData._colorRed;
            _colorGrading.mixerGreenOutBlueIn.value = poisonedData._colorGreen;
            _colorGrading.mixerBlueOutBlueIn.value = poisonedData._colorBlue;
            #endregion
            #region Lens Distortion
            _LensDistortion.intensity.value = poisonedData._lensIntensity;
            #endregion
            #region Vignette
            _vignette.color.value = poisonedData._vignetteColor;
            _vignette.intensity.value = poisonedData._vignetteIntensity;
            _vignette.smoothness.value = poisonedData._vignetteSmoothness;
            _vignette.roundness.value = poisonedData._vignetteRoundness;
            #endregion
            RenderSettings.fog = _postProcessSetting._fogEnabled;
            RenderSettings.fogColor = _postProcessSetting._fogColor;
            if (_postProcessSetting._isFogLinear)
            {
                RenderSettings.fogStartDistance = _postProcessSetting._fogStart;
                RenderSettings.fogEndDistance = _postProcessSetting._fogEnd;
            }
            else
            {
                RenderSettings.fogDensity = _postProcessSetting._fogDensity;
            }
            RenderSettings.subtractiveShadowColor = _postProcessSetting._realtimeShadowColor;
            string _envLightSettings = RenderSettings.ambientMode.ToString();
            if (_envLightSettings == "Flat")
            {
                RenderSettings.ambientSkyColor = _postProcessSetting._ambientrenderColor;
            }
        }
        else
       {
            #region Ambient Occlusion
            _ambientOcclusion.intensity.value = _postProcessSetting._ambientIntensity;
            _ambientOcclusion.thicknessModifier.value = _postProcessSetting._ambientThicknessModifier;
            _ambientOcclusion.color.value = _postProcessSetting._ambientColor;
            _ambientOcclusion.ambientOnly.value = _postProcessSetting._ambientOnly;
            #endregion
            #region Bloom
            _bloom.intensity.value = _postProcessSetting._bloomIntensity;
            _bloom.diffusion.value = _postProcessSetting._bloomDiffusion;
            #endregion
            #region Auto Exposure
            //This place is not necessary for now maybe we will add later
            #endregion
            #region Color Grading
            _colorGrading.hueShift.value = _postProcessSetting._colorHueShift;
            _colorGrading.saturation.value = _postProcessSetting._colorSaturation;
            _colorGrading.brightness.value = _postProcessSetting._colorBrightness;
            _colorGrading.contrast.value = _postProcessSetting._colorContrast;
            _colorGrading.mixerRedOutRedIn.value = _postProcessSetting._colorRed;
            _colorGrading.mixerGreenOutBlueIn.value = _postProcessSetting._colorGreen;
            _colorGrading.mixerBlueOutBlueIn.value = _postProcessSetting._colorBlue;
            #endregion
            #region Lens Distortion
            _LensDistortion.intensity.value = _postProcessSetting._lensIntensity;
            #endregion
            #region Vignette
            _vignette.color.value = _postProcessSetting._vignetteColor;
            _vignette.intensity.value = _postProcessSetting._vignetteIntensity;
            _vignette.smoothness.value = _postProcessSetting._vignetteSmoothness;
            _vignette.roundness.value = _postProcessSetting._vignetteRoundness;
            #endregion
            RenderSettings.fog = _postProcessSetting._fogEnabled;
            RenderSettings.fogColor = _postProcessSetting._fogColor;
            if (_postProcessSetting._isFogLinear)
            {
                RenderSettings.fogStartDistance = _postProcessSetting._fogStart;
                RenderSettings.fogEndDistance = _postProcessSetting._fogEnd;
            }
            else
            {
                RenderSettings.fogDensity = _postProcessSetting._fogDensity;
            }
            RenderSettings.subtractiveShadowColor = _postProcessSetting._realtimeShadowColor;
            string _envLightSettings = RenderSettings.ambientMode.ToString();
            if (_envLightSettings == "Flat")
            {
                RenderSettings.ambientSkyColor = _postProcessSetting._ambientrenderColor;
            }
        }
    }
    public void hurtFlash()
    {
        _vignette.color.value = Color.red;
        _vignette.intensity.value = 0.435f;
        _vignette.smoothness.value = 0.637f;
        _vignette.roundness.value = 1;
    }
    public void setActiveModuleObject(GameObject GO)
    {
        activeModuleObject = GO;
    }
    public GameObject getActiveModuleObject()
    {
        return activeModuleObject;
    }
    public void HandleFPSControllerFootstepSounds(footStepSpecs footstepSpecs)
    {
        //HandleThat
        FPSController F = FPSController.PlayerObject.GetComponent<FPSController>();
        F._usingCustomFootstepSounds = true;
        F.editedFootstepSource = footstepSpecs.clips;
        F.editedFootstepJump = footstepSpecs.jumpSound;
        F.editedFootstepLand = footstepSpecs.landSound;
        F.editeFootstepCooldown = footstepSpecs.footstepDelay;
    }
    public void DontHandleFPSControllerFootstepSounds()
    {
        FPSController F = FPSController.PlayerObject.GetComponent<FPSController>();
        F._usingCustomFootstepSounds = false;
    }
}
