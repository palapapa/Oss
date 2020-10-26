using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerStop : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.Stop();
    }
}
