using System;

using System.Collections.Generic;

using System.Runtime.InteropServices;

using System.Text;

using UnityEngine;


//Script by Samarth Pradeep from SamsidParty - Thank you Bud much love from DmrDev00 <3

public class MainAntiCheat : MonoBehaviour

{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN


    //Native Code

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]

    private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);


    [DllImport("user32.dll", CharSet = CharSet.Unicode)]

    private static extern int GetWindowTextLength(IntPtr hWnd);


    [DllImport("user32.dll")]

    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);


    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


    //Internal Methods

    private string GetWindowText(IntPtr hWnd)

    {

        int size = GetWindowTextLength(hWnd);

        if (size > 0)

        {

            var builder = new StringBuilder(size + 1);

            GetWindowText(hWnd, builder, builder.Capacity);

            return builder.ToString();

        }


        return String.Empty;

    }

    private IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)

    {

        IntPtr found = IntPtr.Zero;

        List<IntPtr> windows = new List<IntPtr>();


        EnumWindows(delegate (IntPtr wnd, IntPtr param)

        {

            if (filter(wnd, param))

            {

                windows.Add(wnd);

            }


            return true;

        }, IntPtr.Zero);


        return windows;

    }

    private IEnumerable<IntPtr> FindWindowsWithText(string titleText)

    {

        return FindWindows(delegate (IntPtr wnd, IntPtr param)

        {

            return GetWindowText(wnd).Contains(titleText);

        });

    }


    /// <summary>

    /// Detects whether cheat engine is running, works without admin

    /// </summary>

    /// <returns>True if cheat engine is running</returns>

    public bool IsCheatEngineRunning()

    {

        var ce = FindWindowsWithText("Cheat Engine");
        var ce1 = FindWindowsWithText("ArtMoney");
        var ce2 = FindWindowsWithText("scanmem");
        var ce3 = FindWindowsWithText("GameConqueror");
        var ce4 = FindWindowsWithText("BitSlicer");
        var ce5 = FindWindowsWithText("Squalr");
        var ce6 = FindWindowsWithText("WeMod");
        var ce7 = FindWindowsWithText("PINCE");
        var ce8 = FindWindowsWithText("GameGuardian");
        var ce9 = FindWindowsWithText("CoSMOS");
        var ce0 = FindWindowsWithText("Memory Hacking Software");

        foreach (var win in ce) { return true; }
        foreach (var win in ce1) { return true; }
        foreach (var win in ce2) { return true; }
        foreach (var win in ce3) { return true; }
        foreach (var win in ce4) { return true; }
        foreach (var win in ce5) { return true; }
        foreach (var win in ce6) { return true; }
        foreach (var win in ce7) { return true; }
        foreach (var win in ce8) { return true; }
        foreach (var win in ce9) { return true; }
        foreach (var win in ce0) { return true; }
        return false;

    }


#else


    public static bool IsCheatEngineRunning()

    {

        //Not supported on this platform

        return false;

    }


#endif

}

