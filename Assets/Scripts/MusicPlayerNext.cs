using UnityEngine;

public class MusicPlayerNext : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.PlayNext(1.0f);
    }
}