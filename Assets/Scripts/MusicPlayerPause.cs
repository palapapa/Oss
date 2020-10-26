using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerPause : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.Pause();
    }
}
