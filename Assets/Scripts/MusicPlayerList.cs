using UnityEngine;

public class MusicPlayerList : MonoBehaviour, ILeftClickable
{
    public SongList SongList;
    public void OnLeftClick()
    {
        StartCoroutine(SongList.Activate());
    }
}
