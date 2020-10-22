using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static List<Song> Songs { get; set; }
    private static IEnumerable<string> osuPaths;
    private static bool isScanningSongDirectory = false;
    private static SongManager instance;

    private void Awake()
    {
        Songs = new List<Song>
        {
            Audio.Instance.Triangles
        };
    }

    private void Update()
    {
    }

    public static IEnumerator ScanSongDirectory()
    {
        Debug.Log("Scanning song directory");
        if (!isScanningSongDirectory)
        {
            isScanningSongDirectory = true;
        }
        else
        {
            yield break;
        }
        Songs.Clear();
        try
        {
            osuPaths = Directory.EnumerateFiles(PlayerData.Instance.BeatmapLocation, "*.osu", SearchOption.AllDirectories);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }
        foreach (string path in osuPaths ?? Enumerable.Empty<string>())
        {
            Beatmap beatmap = BeatmapDecoder.Decode(path);
            string audioPath = path.Substring(0, path.LastIndexOf('\\') + 1) + beatmap.GeneralSection.AudioFilename;
            try
            {
                Song song = new Song(beatmap, Audio.Mp3ToAudioClip(File.ReadAllBytes(audioPath)));
                if (!Songs.Contains(song))
                {
                    Songs.Add(song);
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
            }
            Debug.Log(path);
            yield return null;
        }
        isScanningSongDirectory = false;
    }
    private SongManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}