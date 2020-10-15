using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuParsers.Beatmaps;

public class Song : Beatmap
{
    public AudioClip AudioClip { get; set; }
    public Song(AudioClip ac) : base()
    {
        AudioClip = ac;
    }
    public Song() : base()
    {
        AudioClip = null;
    }
}
