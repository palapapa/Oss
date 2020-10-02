using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class SelectBeatmapLocation : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        string[] path = StandaloneFileBrowser.OpenFolderPanel("Select Beatmap Location(Songs folder)", @"C:\Users\" + System.Environment.UserName + @"\AppData\Local\osu!", false);
        if (path.Length != 0)
        {
            Data.BeatmapLocation = path[0];
        }
        Debug.Log(Data.BeatmapLocation);
    }
}
