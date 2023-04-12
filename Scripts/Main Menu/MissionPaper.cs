using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionPaper : MonoBehaviour
{
    public TextMeshProUGUI TMPU;
    public RandomRooms myRooms;
    private void Start()
    {
        int[] minModules = { 0, 1, 0, 0 };
        int[] minMaxModules = { 0, 1, 1, 1 };
        int[] MaxModules = { 2, 2, 2, 2 };
        myRooms = MainMenuManager.instance.createQuest(1, 50, 0, 0, minModules, minMaxModules, MaxModules, true,1);
        int mySides = myRooms.minLab + myRooms.minMarket + myRooms.minBlacksmith;
        TMPU.text = "Mercy count: " + myRooms.mercyCount + "\nTotal sectors: "+ myRooms.maxSector  + "\nTotal locations per sector: " + myRooms.maxRooms + "\nminimum main count:"+ myRooms.minMain;
    }
    public void initLevel()
    {
        MainMenuManager.instance.infoPiercer.roomStats = myRooms;
        Application.LoadLevel(1);
    }
}
