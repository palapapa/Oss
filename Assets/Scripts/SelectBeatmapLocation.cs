﻿using SFB;
using UnityEngine;

public class SelectBeatmapLocation : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        string[] path = StandaloneFileBrowser.OpenFolderPanel("Select Beatmap Location(Songs folder)", @"C:\Users\" + System.Environment.UserName + @"\AppData\Local\osu!", false);
        if (path.Length != 0)
        {
            PlayerData.PersistentPlayerData.BeatmapLocation = path[0];
            PlayerData.SavePersistentPlayerData();
        }
    }
}