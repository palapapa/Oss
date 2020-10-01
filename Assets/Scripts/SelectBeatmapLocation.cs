using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBeatmapLocation : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        Data.BeatmapLocation = UnityEditor.EditorUtility.OpenFolderPanel("Select Beatmap Location", @"C:\Users\" + System.Environment.UserName + @"\AppData\Local\osu!", "Songs");
        Debug.Log(Data.BeatmapLocation);
    }
}
