using UnityEngine;

public class MusicPlayerPause : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.Pause();
    }
}