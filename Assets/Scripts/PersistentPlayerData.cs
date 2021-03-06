﻿[System.Serializable]
public class PersistentPlayerData
{
    public string BeatmapLocation { get; set; }

    /// <summary>
    /// Don't use this variable as volume in <code>Audio.PlayAudio</code> Always use 1.0f as volume instead.
    /// </summary>
    public float MusicVolume { get; set; }

    /// <summary>
    /// Don't use this variable as volume in <code>Audio.PlayAudio</code> Always use 1.0f as volume instead.
    /// </summary>
    public float SoundEffectsVolume { get; set; }
    public float MasterVolume { get; set; }

    public PersistentPlayerData()
    {
        BeatmapLocation = string.Empty;
        MusicVolume = 1.0f;
        SoundEffectsVolume = 1.0f;
        MasterVolume = 1.0f;
    }
}