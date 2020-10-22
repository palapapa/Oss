using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerLast : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.PlayLast(PlayerData.Instance.MusicVolume);
    }
}
