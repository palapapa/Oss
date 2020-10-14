using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Contains utilities related to the music player in the main menu, use the methods in this class if you want to play a music and have it registered to the main menu music player
public class PlayList : MonoBehaviour
{
    public static AudioClip CurrentPlaying { get; set; }
    public Text MusicName;
    
    void Start()
    {
        MusicName.text = "Triangles";//better way?
    }

    void Update()
    {
        
    }

    /*
    public static GameObject PlayRandom(float volume)
    {
        AudioClip ac = Audio.Instance.Musics[new System.Random().Next()];
        CurrentPlaying = ac;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }
    */
    public static GameObject PlaySelected(AudioClip ac, float volume)
    {
        CurrentPlaying = ac;
        return Audio.PlayAudio(ac, volume, AudioChannels.Music);
    }
}