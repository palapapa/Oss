using OsuParsers.Beatmaps;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using OsuParsers.Beatmaps.Objects;

public class Song : Beatmap
{
    public AudioClip AudioClip { get; set; }

    public Song(AudioClip audioClip) : base()
    {
        AudioClip = audioClip;
    }

    public Song(Beatmap beatmap)
    {
        Version = beatmap.Version;
        GeneralSection = beatmap.GeneralSection;
        EditorSection = beatmap.EditorSection;
        MetadataSection = beatmap.MetadataSection;
        DifficultySection = beatmap.DifficultySection;
        EventsSection = beatmap.EventsSection;
        ColoursSection = beatmap.ColoursSection;
        TimingPoints = new List<TimingPoint>(beatmap.TimingPoints);
        HitObjects = new List<HitObject>(beatmap.HitObjects);
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