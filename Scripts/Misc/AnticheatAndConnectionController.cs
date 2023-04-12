using System.Threading;
using UnityEngine;
using System.Net.NetworkInformation;
using System;
using Ping = System.Net.NetworkInformation.Ping;
using System.Collections.Generic;
using Color = UnityEngine.Color;

public class AnticheatAndConnectionController : MonoBehaviour
{
    [SerializeField]
    int failedInternetCharges; //Max 3 Fail Before Closing
    bool cheatDetected;
    List<Action> mainThreadThings = new List<Action>();
    [SerializeField] MainAntiCheat mac;
    void Start()
    {
        InvokeRepeating("repeatCycle", 15,30);
    }
    private void Update()
    {
        while (mainThreadThings.Count > 0)
        {
            Action someFunc = mainThreadThings[0];
            mainThreadThings.RemoveAt(0);
            someFunc();
        }
    }
    public void repeatCycle()
    {
        Thread helperThread = new Thread(checkAnticheat);
        helperThread.Start();
        Thread helperThread2 = new Thread(checkConnection);
        helperThread2.Start();
    }
    public void checkConnection()
    {
        try
        {
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                UnityEngine.Debug.Log("Internet ping is success");
                if (failedInternetCharges > 0)
                {
                    failedInternetCharges = 0;
                    Action myAction = () =>
                    {
                        FadingTextCreator.instance.CreateFadeText("Internet Connection Detected!", 5f, 5, null, Color.green);
                    };
                    mainThreadThings.Add(myAction);
                }
            }
            else
            {
                UnityEngine.Debug.Log("failed");
                if (failedInternetCharges < 3)
                {
                    Action myAction = () =>
                    {
                        FadingTextCreator.instance.CreateFadeText("Pinging To Web Is Failed Please Check Your Internet Connection!\n" + "3/" + failedInternetCharges, 5f, 5, null, Color.red);
                    };
                mainThreadThings.Add(myAction);

                failedInternetCharges += 1;
                }
                else
                {
                    Action myAction = () =>
                    {
                        closeGame();
                    };
                    mainThreadThings.Add(myAction);
                }
            }
        }
        catch (Exception err)
        {
            UnityEngine.Debug.Log("failed");
            if (failedInternetCharges < 3)
            {
                Action myAction = () =>
                {
                    FadingTextCreator.instance.CreateFadeText("Pinging To Web Is Failed Please Check Your Internet Connection!\n" + "3/" + failedInternetCharges, 5f, 5, null, Color.red);
                };
                mainThreadThings.Add(myAction);
                failedInternetCharges += 1;
            }
            else
            {
                Action myAction = () =>
                {
                    closeGame();
                };
                mainThreadThings.Add(myAction);
            }
        }
    }
    private void checkAnticheat()
    {
        UnityEngine.Debug.Log("Cheat Checking..");
        bool myBool =
        mac.IsCheatEngineRunning();
        if (myBool && !cheatDetected)
        {
            cheatDetected = true;
            detectedCheat();
        }
    }
    private void detectedCheat()
    {
        Action myAction = () =>
        {
            FadingTextCreator.instance.CreateFadeText("CAN'T YOU PLAY THIS GAME NORMALLY? WHAT A SHAME", 5f, 400f, null, Color.red);
            if (MainDatabaseManager.instance == null)
            {
                Invoke("closeGame", 5f);
                return;
            }
            if (MainDatabaseManager.instance.closeDATABASE)
            {
                Invoke("closeGame", 5f);
            }else
            {
                MainDatabaseManager.instance.banAndCloseGame(MainDatabaseManager.instance.databasePiercer.playerID);
            }
            UnityEngine.Debug.Log("Cheat Detected");
        };
        mainThreadThings.Add(myAction);
    }
    public void closeGame()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        //Make Ban To Database If You Want
    }
}
