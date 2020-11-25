using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongListButton : MonoBehaviour, ILeftClickable
{
    public Text Title;
    public void OnLeftClick()
    {
        MusicPlayer.PlaySelected(Title.text, 1.0f);
    }
}
