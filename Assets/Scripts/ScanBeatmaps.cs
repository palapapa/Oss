using UnityEngine;

public class ScanBeatmaps : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        StartCoroutine(SongManager.ScanSongDirectory());
    }
}