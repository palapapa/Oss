using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using System.IO;
using System.Linq;
using System;

public class SongManager : MonoBehaviour
{
    public static List<Song> Songs { get; set; }
    private static IEnumerable<string> audioPaths;
    private static bool isScanningSongDirectory = false;
    void Start()
    {
        Songs = new List<Song>();
        if (!isScanningSongDirectory)
        {
            StartCoroutine(ScanSongDirectory());
        }
        else // force reset the coroutine
        {
            StopCoroutine(nameof(ScanSongDirectory));
            isScanningSongDirectory = false;
            StartCoroutine(ScanSongDirectory());
        }
    }
    void Update()
    {
        
    }
    public static IEnumerator ScanSongDirectory()
    {
        if (!isScanningSongDirectory)
        {
            isScanningSongDirectory = true;
        }
        Songs.Clear();
        try
        {
            audioPaths = Directory.EnumerateFiles(PlayerData.Instance.BeatmapLocation, "*.osu", SearchOption.AllDirectories);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }
        foreach (string path in audioPaths ?? Enumerable.Empty<string>())
        {
            Beatmap beatmap = BeatmapDecoder.Decode(path);
            string audioPath = path.Substring(0, path.LastIndexOf('\\') + 1) + beatmap.GeneralSection.AudioFilename;
            try
            {
                Songs.Add(new Song(beatmap, Audio.Mp3ToAudioClip(File.ReadAllBytes(audioPath))));
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
}