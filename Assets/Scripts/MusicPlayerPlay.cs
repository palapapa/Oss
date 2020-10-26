using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerPlay : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.Play();
    }
}
