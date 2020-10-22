using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerNext : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        MusicPlayer.PlayNext(PlayerData.Instance.MusicVolume);
    }
}
