using UnityEngine;

public class MusicPlayerLast : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.PlayLast(1.0f);
    }
}