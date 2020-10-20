using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBeatmaps : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        StartCoroutine(SongManager.ScanSongDirectory());
    }
}
