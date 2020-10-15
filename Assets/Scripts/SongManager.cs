using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using System.IO;

public class SongManager : MonoBehaviour
{
    public static List<Song> Songs { get; set; }
    void Start()
    {
        List<string> paths = (List<string>)Directory.EnumerateFiles(PlayerData.Instance.BeatmapLocation, "*.osu", SearchOption.AllDirectories);
    }

    void Update()
    {
        
    }
}
