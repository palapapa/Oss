using NAudio.Wave;
using OsuParsers.Beatmaps.Sections;
using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Contains some in-game sound effects and provides audio-related utilities, use the methods in this class if you simply want to play a sound and nothing more
/// </summary>
public class Audio : MonoBehaviour
{
    public static Audio Instance { get; set; }
    public AudioClip TrianglesAudioClip;
    public Song Triangles { get; set; }
    public AudioClip KeyDown0;
    public AudioClip KeyDown1;
    public AudioClip KeyDown2;
    public AudioClip Backspace;
    public AudioClip[] KeyDownSounds { get; set; }
    private bool hasLoaded = false;

    private void Awake()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
        KeyDownSounds = new AudioClip[] { KeyDown0, KeyDown1, KeyDown2 };
        Triangles = new Song(Instance.TrianglesAudioClip)
        {
            MetadataSection = new BeatmapMetadataSection()
            {
                TitleUnicode = "Triangles",
                ArtistUnicode = "cYsmix",
                Title = "Triangles",
                Artist = "cYsmix"
            }
        };
    }

    private void Start()
    {
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

    public static AudioClip Mp3ToAudioClip(byte[] data)
    {
        // Load the data into a stream
        MemoryStream mp3stream = new MemoryStream(data);
        // Convert the data in the stream to WAV format
        Mp3FileReader mp3audio = new Mp3FileReader(mp3stream);
        WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3audio);
        // Convert to WAV data
        WAV wav = new WAV(AudioMemoryStream(waveStream).ToArray());
        AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
        audioClip.SetData(wav.LeftChannel, 0);
        // Return the clip
        return audioClip;
    }

    private static MemoryStream AudioMemoryStream(WaveStream waveStream)
    {
        MemoryStream outputStream = new MemoryStream();
        using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
        {
            byte[] bytes = new byte[waveStream.Length];
            waveStream.Position = 0;
            waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
            waveFileWriter.Write(bytes, 0, bytes.Length);
            waveFileWriter.Flush();
        }
        return outputStream;
    }
}