using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SongManager : MonoBehaviour
{
    public static List<Song> Songs { get; set; }
    private static IEnumerable<string> osuPaths;
    private static bool isScanningSongDirectory = false;
    private static SongManager instance;

    private void Start()
    {
        Songs = new List<Song>
        {
            Audio.Instance.Triangles
        };
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
        Songs.Add(Audio.Instance.Triangles);
        try
        {
            osuPaths = Directory.EnumerateFiles(PlayerData.PersistentPlayerData.BeatmapLocation, "*.osu", SearchOption.AllDirectories);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }
        foreach (string path in osuPaths ?? Enumerable.Empty<string>())
        {
            Beatmap beatmap = BeatmapDecoder.Decode(path);
            string songFolderPath = path.Substring(0, path.LastIndexOf('\\') + 1);
            string audioPath = songFolderPath + beatmap.GeneralSection.AudioFilename;
            string backgroundPath = songFolderPath + beatmap.EventsSection.BackgroundImage;
            try
            {
                Song song = new Song(beatmap, Audio.Mp3ToAudioClip(File.ReadAllBytes(audioPath)));
                //song.Background.LoadImage(File.ReadAllBytes(backgroundPath));
                Songs.Add(song);
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
            }
            Debug.Log(path);
            yield return null;
        }
        isScanningSongDirectory = false;
        SongList.Instance.UpdateSongList();
        SongSelection.Instance.UpdateSongList();
    }

    public static List<Song> GetUniqueSongList()
    {
        List<Song> result = new List<Song>();
        for (int i = 0; i < Songs.Count; i++)
        {
            if (!result.Exists((s) => s.MetadataSection.Title == Songs[i].MetadataSection.Title))
            {
                result.Add(Songs[i]);
            }
        }
        return result;
    }

    private SongManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}