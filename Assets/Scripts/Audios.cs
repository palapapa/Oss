using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audios : MonoBehaviour
{
    public static Audios Instance;//workaround to show audio clips in inspector
    public AudioClip Triangles;
    public AudioClip KeyDown0;
    public AudioClip KeyDown1;
    public AudioClip KeyDown2;
    [HideInInspector]
    public AudioClip[] KeyDownSounds;
    public AudioClip Backspace;
    public static GameObject Music;
    public static GameObject SoundEffects;
    private static bool hasLoaded = false;
    private void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
        KeyDownSounds = new AudioClip[] { KeyDown0, KeyDown1, KeyDown2 };
        Music = new GameObject("Music");
        Music.transform.SetParent(transform);
        SoundEffects = new GameObject("SoundEffects");
        SoundEffects.transform.SetParent(transform);
    }

    private Audios()
    {
        Instance = this;
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
