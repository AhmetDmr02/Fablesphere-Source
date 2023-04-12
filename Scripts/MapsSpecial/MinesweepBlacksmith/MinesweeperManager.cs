using HelperOfDmr;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MinesweeperManager : MonoBehaviour,IMapGetSerializer
{
    [SerializeField] private EnemyHitCenter EHC;
    [SerializeField] private int row,minMines,maxMines;
    [SerializeField] private int leftToWin,currentMines;
    [SerializeField] private List<int> unveiledMines = new List<int>();
    public List<MinesweeperMine> allTheMines = new List<MinesweeperMine>();
    bool firstVeilOpened,wonalready;
    private void Start()
    {
        ProceduralModuleGenerator.lastIndexFired += serializeMe;
    }
    public void openMine(MinesweeperMine mm)
    {
        //Kill player
        if (wonalready) return;
        EHC.HitPlayer();
        foreach (MinesweeperMine mine in allTheMines)
        {
            mine.removeSerialization();
        }
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }
    public void giveMines(MinesweeperMine origin)
    {
        currentMines = Random.Range(minMines, maxMines + 1);
        if (currentMines > allTheMines.Count) 
        { Debug.LogError("NOT ENOUGH LENGTH TO OPEN MINES!"); return; }
        List<MinesweeperMine> temperList = allTheMines.ToList();
        temperList.Remove(origin);
        for (int i = 0; i < currentMines; i++)
        {
            temperList.Shuffle();
            temperList[0].isThisMine = true;
            temperList.RemoveAt(0);
        }
        foreach (MinesweeperMine mine in allTheMines)
        {
            if (!mine.isThisMine && mine != origin)
            {
                unveiledMines.Add(allTheMines.IndexOf(mine));
            }
        }
        int minecount = checkMinecount(origin);
        origin.openVeil(minecount);
        leftToWin = unveiledMines.Count;
        CheckForWin();
    }
    public void revealArea(MinesweeperMine mm)
    {
        if (!firstVeilOpened)
        {
            firstVeilOpened = true;
            giveMines(mm);
            return;
        }
        if (mm.mineOpened) return;
        if (wonalready) return;
        int minecount = checkMinecount(mm);
        mm.openVeil(minecount);
        int indexRemove = allTheMines.IndexOf(mm);
        int indexRemove2 = unveiledMines.IndexOf(indexRemove);
        unveiledMines.RemoveAt(indexRemove2);
        leftToWin = unveiledMines.Count;
        CheckForWin();
        if (minecount == 0)
        {
            List<MinesweeperMine> revealtrick = returnListOfCalculatedMines(mm);
            revealTrick(revealtrick);
        }
    }
    public int checkMinecount(MinesweeperMine mm)
    {
        int mineCount = 0;
        List<MinesweeperMine> minesss = returnListOfCalculatedMines(mm);
        foreach (MinesweeperMine m in minesss)
        {
            if (m.isThisMine)
            {
                mineCount += 1;
            }
        }
        Debug.Log("Found Mines : " + mineCount);
        return mineCount;
    }
    public void revealTrick(List<MinesweeperMine> mm)
    {
        foreach (MinesweeperMine mine in mm)
        {
            revealArea(mine);
        }
    }
    public void CheckForWin()
    {
        if (leftToWin <= 0)
        {
            MinesweepModuleMain.instance.openDoorIndexes[0] = 1;
            MinesweepModuleMain.instance.doors[0].SetActive(false);
            wonalready = true;
        }
    }
    public void serializeMe(int index)
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
        SerializationInfoClass SIC = UtilitesOfDmr.CreateDefaultSIC(this.gameObject,this);
        SIC.serialIntList.Add(unveiledMines.Count);
        for (int i = 0; i < unveiledMines.Count; i++)
        {
            SIC.serialIntList.Add(unveiledMines[i]);
        }
        SIC.serialIntList.Add(leftToWin);
        SIC.serialIntList.Add(currentMines);
        SIC.serialBoolList.Add(firstVeilOpened);
        SIC.serialBoolList.Add(wonalready);
        ProceduralModuleGenerator.instance.saveMapByInfoClassAndMapIndex(SIC, ProceduralModuleGenerator.instance.currentIndex);
    }
    public void GetMapInfoClass(SerializationInfoClass SIC)
    {
        firstVeilOpened = SIC.serialBoolList[0];
        wonalready = SIC.serialBoolList[1];
        int count = SIC.serialIntList[0];
        for (int i = 1; i <= count; i++)
        {
            unveiledMines.Add(SIC.serialIntList[i]);
        }
        leftToWin = SIC.serialIntList[count + 1];
        currentMines = SIC.serialIntList[count + 2];
    }
    private void OnDestroy()
    {
        ProceduralModuleGenerator.lastIndexFired -= serializeMe;
    }

    //Calculation Part
    public List<MinesweeperMine> returnListOfCalculatedMines(MinesweeperMine origin)
    {
        List<MinesweeperMine> ThreeRadiusOfMines = new List<MinesweeperMine>();
        int indexOfMain = allTheMines.IndexOf(origin);
        int countOfMain = indexOfMain + 1;
        bool checkForUp = false;
        bool checkForDown = false;
        bool checkForRight = false;
        bool checkForLeft = false;
        if (countOfMain % row == 1)
        {
            checkForRight = true;
            checkForLeft = false;
        }
        else if (countOfMain % row == 0)
        {
            checkForRight = false;
            checkForLeft = true;
        }
        else if (countOfMain % row > 1)
        {
            checkForLeft = true;
            checkForRight = true;
        }
        if (countOfMain + row <= allTheMines.Count)
        {
            checkForDown = true;
        }
        if (countOfMain - row >= 1)
        {
            checkForUp = true;
        }
        Debug.Log("Checkforup = " + checkForUp + " Checkfordown = " + checkForDown + " Checkforleft = " + checkForLeft + " Checkforright = " + checkForRight);
        //WELCOME THE ELSE-IF HELL
        if (checkForUp)
        {
            ThreeRadiusOfMines.Add(allTheMines[countOfMain - row - 1]);
            Debug.Log("Trying For " + allTheMines[countOfMain - row - 1].name + " in index of " + (countOfMain - row - 1).ToString());
            //Diagonals
            if(checkForLeft) ThreeRadiusOfMines.Add(allTheMines[countOfMain - row - 2]);
            if(checkForRight) ThreeRadiusOfMines.Add(allTheMines[countOfMain - row]);
            if (checkForLeft) Debug.Log("Trying For " + allTheMines[countOfMain - row - 2].name + " in index of " + (countOfMain - row - 2).ToString());
            if (checkForRight) Debug.Log("Trying For " + allTheMines[countOfMain - row ].name + " in index of " + (countOfMain - row).ToString());
        }
        if (checkForDown)
        {
            ThreeRadiusOfMines.Add(allTheMines[countOfMain + row - 1]);
            Debug.Log("Trying For " + allTheMines[countOfMain + row - 1].name + " in index of " + (countOfMain + row - 1).ToString());   
            //Diagonals
            if (checkForLeft) ThreeRadiusOfMines.Add(allTheMines[countOfMain + row - 2]);
            if (checkForRight) ThreeRadiusOfMines.Add(allTheMines[countOfMain + row]);
            if (checkForLeft) Debug.Log("Trying For " + allTheMines[countOfMain + row - 2].name + " in index of " + (countOfMain + row - 2).ToString());
            if (checkForRight) Debug.Log("Trying For " + allTheMines[countOfMain + row].name + " in index of " + (countOfMain + row).ToString());
        }
        if (checkForLeft) ThreeRadiusOfMines.Add(allTheMines[countOfMain - 2]);
        if (checkForRight) ThreeRadiusOfMines.Add(allTheMines[countOfMain]);
        if (checkForLeft) Debug.Log("Trying For " + allTheMines[countOfMain - 2].name + " in index of " + (countOfMain - 2).ToString());
        if (checkForRight) Debug.Log("Trying For " + allTheMines[countOfMain].name + " in index of " + (countOfMain).ToString());
        return ThreeRadiusOfMines;
        
    }
}
