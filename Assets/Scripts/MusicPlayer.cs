using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Sections;
using System;
using UnityEngine;
using UnityEngine.UI;

//Contains utilities related to the music player in the main menu, use the methods in this class if you want to play a music and have it registered to the main menu music player
public class MusicPlayer : MonoBehaviour
{
    public static Song CurrentPlaying { get; set; }
    public Text MusicName;
    public GameObject MainMenu;
    private static AudioSource musicAudioSource;

    private void Start()
    {
        CurrentPlaying = Audio.Instance.Triangles;
        MusicName.text = CurrentPlaying.MetadataSection.TitleUnicode;
        musicAudioSource = AudioChannels.Music.GetComponent<AudioSource>();
    }

    private void Update()
    {
        MusicName.text = CurrentPlaying.MetadataSection.TitleUnicode;
        if (!musicAudioSource.isPlaying && PlayerData.Instance.HasIntroFinished && PlayerData.Instance.ActivePanel == MainMenu)
        {
            try
            {
                PlayNext(PlayerData.Instance.MusicVolume);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.Log("Can't find next song");
            }
        }
    }

    public static GameObject PlayRandom(float volume)
    {
        Song song = SongManager.Songs[new System.Random().Next(0, SongManager.Songs.Count)];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        musicAudioSource.Stop();
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }
    public static GameObject PlayNext(float volume)
    {
        int index = SongManager.Songs.FindIndex(s => s == CurrentPlaying) + 1;
        if (index >= SongManager.Songs.Count)
        {
            index = 0;
        }
        Song song = SongManager.Songs[index];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        musicAudioSource.Stop();
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }
    public static GameObject PlayLast(float volume)
    {
        int index = SongManager.Songs.FindIndex(s => s == CurrentPlaying) - 1;
        if (index < 0)
        {
            index = SongManager.Songs.Count - 1;
        }
        Song song = SongManager.Songs[index];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        musicAudioSource.Stop();
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }
    public static GameObject PlaySelected(Song song, float volume)
    {
        if (!SongManager.Songs.Exists(s => s == song))
        {
            CurrentPlaying = song;
            musicAudioSource.Stop();
            return Audio.PlayAudio(song.AudioClip, volume, AudioChannels.Music);
        }
        else
        {
            throw new ArgumentException($"{song.MetadataSection.TitleUnicode} is not registered in SongManager.Songs");
        }
    }
}