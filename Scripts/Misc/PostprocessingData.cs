using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "New Post Process Stat", menuName = "Create new post process setting")]
public class PostprocessingData : ScriptableObject
{
    [Header("Ambient Occlusion")]
    public float _ambientIntensity;
    public float _ambientThicknessModifier;
    public Color _ambientColor;
    public bool _ambientOnly;
    [Header("Bloom")]
    public float _bloomIntensity;
    public float _bloomDiffusion = 7f;
    [Header("Auto Exposure")]
    //empty for now
    [Header("Color Grading")]
    public float _colorHueShift;
    public float _colorSaturation;
    public float _colorBrightness;
    public float _colorContrast;
    public float _colorRed;
    public float _colorGreen;
    public float _colorBlue;
    [Header("LensDistortion")]
    public float _lensIntensity;
    [Header("Vignette")]
    public Color _vignetteColor;
    public float _vignetteIntensity;
    public float _vignetteSmoothness;
    public float _vignetteRoundness;
    [Header("Renderer Settings")]
    public bool _fogEnabled;
    [HideInInspector]
    public bool _isFogLinear = false;
    public Color _fogColor;
    [HideInInspector]
    public float _fogDensity;
    [HideInInspector]
    public float _fogStart;
    [HideInInspector]
    public float _fogEnd;
    public Color _realtimeShadowColor;
    public Color _ambientrenderColor;

    #if UNITY_EDITOR
    [CustomEditor(typeof(PostprocessingData))]
    public class FogHider : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myScript = target as PostprocessingData;

            myScript._isFogLinear = GUILayout.Toggle(myScript._isFogLinear, "isFogLinear");

            if (myScript._isFogLinear)
            {
                myScript._fogStart = EditorGUILayout.FloatField("Fog Start Var:", myScript._fogStart);
                myScript._fogEnd = EditorGUILayout.FloatField("Fog End Var:", myScript._fogEnd);
            }else
            {
                myScript._fogDensity = EditorGUILayout.FloatField("Fog Density Var:", myScript._fogDensity);
            }

        }
    }
    #endif
}