using OsuParsers.Beatmaps;
using UnityEngine;
using UnityEngine.UI;

//Contains utilities related to the music player in the main menu, use the methods in this class if you want to play a music and have it registered to the main menu music player
public class PlayList : MonoBehaviour
{
    public static Beatmap CurrentPlaying { get; set; }
    public Text MusicName;

    private void Start()
    {
        MusicName.text = "cYsmix - Triangles";//better way?
    }

    private void Update()
    {
        CurrentPlaying = Audio.Instance.Triangles;
        MusicName.text = CurrentPlaying.MetadataSection.TitleUnicode;
    }

    public static GameObject PlayRandom(float volume)
    {
        Song song = SongManager.Songs[new System.Random().Next(0, SongManager.Songs.Count)];
        AudioClip ac = song.AudioClip;
        CurrentPlaying = song;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }

    public static GameObject PlaySelected(Song song, float volume)
    {
        CurrentPlaying = song;
        return Audio.PlayAudio(song.AudioClip, volume, AudioChannels.Music);
    }
}