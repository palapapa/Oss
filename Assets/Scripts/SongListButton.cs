using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongListButton : MonoBehaviour, ILeftClickable
{
    private Outline outline;
    public Text Title;
    public Song Song { get; set; }

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void Update()
    {
        outline.enabled = Title.text == MusicPlayer.CurrentPlaying.MetadataSection.Title;
    }

    public void OnLeftClick()
    {
        if (MusicPlayer.CurrentPlaying.MetadataSection.Title != Song.MetadataSection.Title)
        {
            MusicPlayer.PlaySelected(Song, 1.0f);
        }
    }
}
