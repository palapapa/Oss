using System.Collections.Generic;
using UnityEngine;
using OsuParsers;
using OsuParsers.Beatmaps;

//Contains some in-game sound effects and provides audio-related utilities, use the methods in this class if you simply want to play a sound and nothing more
public class Audio : MonoBehaviour
{
    public static Audio Instance { get; set; }
    public AudioClip Triangles;
    public AudioClip KeyDown0;
    public AudioClip KeyDown1;
    public AudioClip KeyDown2;
    public AudioClip Backspace;
    public AudioClip[] KeyDownSounds { get; set; }
    public List<Beatmap> Musics { get; set; }
    private bool hasLoaded = false;

    private void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
        KeyDownSounds = new AudioClip[] { KeyDown0, KeyDown1, KeyDown2 };
        Musics = new List<Beatmap>();
    }

    private Audio()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static GameObject PlayAudio(AudioClip ac)
    {
        GameObject gameObject = new GameObject(ac.name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(ac);
        return gameObject;
    }

    public static GameObject PlayAudio(AudioClip ac, float volume)
    {
        GameObject gameObject = new GameObject(ac.name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(ac, volume);
        return gameObject;
    }

    public static GameObject PlayAudio(AudioClip ac, float volume, string name)
    {
        GameObject gameObject = new GameObject(name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(ac, volume);
        return gameObject;
    }

    public static GameObject PlayAudio(AudioClip ac, float volume, GameObject channel)
    {
        AudioSource audioSource = channel.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = channel.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(ac, volume);
        return channel;
    }
}