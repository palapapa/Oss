using OsuParsers.Beatmaps;
using UnityEngine;

public class Song : Beatmap
{
    public AudioClip AudioClip { get; set; }

    public Song(AudioClip audioClip) : base()
    {
        AudioClip = audioClip;
    }

    public Song(Beatmap beatmap)
    {
        GeneralSection = beatmap.GeneralSection;
        EditorSection = beatmap.EditorSection;
        MetadataSection = beatmap.MetadataSection;
        DifficultySection = beatmap.DifficultySection;
        EventsSection = beatmap.EventsSection;
        ColoursSection = beatmap.ColoursSection;
    }

    public Song(Beatmap beatmap, AudioClip audioClip) : this(beatmap)
    {
        AudioClip = audioClip;
    }

    public Song() : base()
    {
        AudioClip = null;
    }
}