using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Scene Properties", menuName = "Scene Properties")]
public class DataScene : ScriptableObject
{
    public Sprite loadingBanner;
    public string SceneName;
    [TextArea]
    public string SceneExplanation;
    public string scenePath;
    public sceneKind _sceneKind;
}

public enum sceneKind
{
    Lab,
    Blacksmith,
    MainHall,
    Enterance,
    EndScene,
    BossScene,
    marketScene,
}