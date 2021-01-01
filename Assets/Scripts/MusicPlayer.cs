using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains utilities related to the music player in the main menu, use the methods in this class if you want to play a music and have it registered to the main menu music player
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static Song CurrentPlaying { get; set; }
    private static bool HasManuallyPaused { get; set; } = false;
    private static bool HasStopped { get; set; } = false;
    public Text MusicName;
    public GameObject MainMenu;
    private static AudioSource musicAudioSource;

    private void Start()
    {
        CurrentPlaying = Audio.Instance.Triangles;
        MusicName.text = CurrentPlaying.MetadataSection.Title;
        musicAudioSource = AudioChannels.Music.GetComponent<AudioSource>();
    }

    private void Update()
    {
        MusicName.text = CurrentPlaying.MetadataSection.Artist + " - " + CurrentPlaying.MetadataSection.Title;
        if (!musicAudioSource.isPlaying && !HasManuallyPaused && PlayerData.HasIntroFinished)
        {
            try
            {
                PlayNext(1.0f);
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
        HasManuallyPaused = false;
        HasStopped = false;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }

    public static GameObject PlayNext(float volume)
    {
        int index = SongManager.Songs.FindIndex(s => s == CurrentPlaying) + 1;
        if (index >= SongManager.Songs.Count)
        {
            index = 0;
        }
        int originalIndex = index; // the original index before searching for the next unindentical song
        if (SongManager.Songs.Count > 1)
        {
            int lastIndex = index - 1; // the index of the previous song before switching
            if (lastIndex < 0)
            {
                lastIndex = SongManager.Songs.Count - 1;
            }
            while (SongManager.Songs[lastIndex].MetadataSection.Title == SongManager.Songs[index].MetadataSection.Title) // if the last song is the same song
            {
                index++;
                if (index >= SongManager.Songs.Count)
                {
                    index = 0;
                }
                if (index == originalIndex) // if a whole loop has been made
                {
                    break;
                }
            }
        }
        Song song = SongManager.Songs[index];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        musicAudioSource.Stop();
        HasManuallyPaused = false;
        HasStopped = false;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }

    public static GameObject PlayLast(float volume)
    {
        int index = SongManager.Songs.FindIndex(s => s == CurrentPlaying) - 1;
        if (index < 0)
        {
            index = SongManager.Songs.Count - 1;
        }
        int originalIndex = index;
        if (SongManager.Songs.Count > 1)
        {
            int lastIndex = index + 1; // the index of the previous song before switching
            if (lastIndex >= SongManager.Songs.Count)
            {
                lastIndex = 0;
            }
            while (SongManager.Songs[lastIndex].MetadataSection.Title == SongManager.Songs[index].MetadataSection.Title)
            {
                index--;
                if (index < 0)
                {
                    index = SongManager.Songs.Count - 1;
                }
                if (index == originalIndex)// if a whole loop has been made
                {
                    break;
                }
            }
        }
        Song song = SongManager.Songs[index];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        musicAudioSource.Stop();
        HasManuallyPaused = false;
        HasStopped = false;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }

    public static GameObject PlaySelected(Song song, float volume)
    {
        if (SongManager.Songs.Exists(s => s == song))
        {
            CurrentPlaying = song;
            musicAudioSource.Stop();
            HasManuallyPaused = false;
            HasStopped = false;
            return Audio.PlayAudio(song.AudioClip, volume, AudioChannels.Music);
        }
        else
        {
            throw new ArgumentException($"{song.MetadataSection.Title} is not registered in SongManager.Songs", nameof(song));
        }
    }

    public static GameObject PlaySelected(string title, float volume)
    {
        Song song = SongManager.Songs.Find(s => s.MetadataSection.Title == title);
        if (song != null)
        {
            CurrentPlaying = song;
            musicAudioSource.Stop();
            HasManuallyPaused = false;
            HasStopped = false;
            return Audio.PlayAudio(song.AudioClip, volume, AudioChannels.Music);
        }
        else
        {
            throw new ArgumentException($"{song.MetadataSection.Title} is not registered in SongManager.Songs", nameof(title));
        }
    }

    public static void Stop()
    {
        musicAudioSource.Stop();
        HasManuallyPaused = true;
        HasStopped = true;
    }

    public static void Pause()
    {
        musicAudioSource.Pause();
        HasManuallyPaused = true;
    }

    public static void Play()
    {
        if (!HasManuallyPaused)
        {
            musicAudioSource.Stop();
            PlaySelected(CurrentPlaying, 1.0f);
            HasManuallyPaused = false;
        }
        else if (HasStopped)
        {
            musicAudioSource.Stop();
            PlaySelected(CurrentPlaying, 1.0f);
            HasManuallyPaused = false;
            HasStopped = false;
        }
        else
        {
            musicAudioSource.UnPause();
            HasManuallyPaused = false;
        }
    }
}