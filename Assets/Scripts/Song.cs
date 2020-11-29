using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Song : Beatmap
{
    public AudioClip AudioClip { get; set; }
    public Texture2D Background { get; set; }

    public Song() : base()
    {
        AudioClip = null;
        Background = Texture2D.whiteTexture;
        for (int x = 0; x < Background.width; x++)
        {
            for (int y = 0; y < Background.height; y++)
            {
                Background.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
    }

    public Song(AudioClip audioClip) : this()
    {
        AudioClip = audioClip;
    }

    public Song(Beatmap beatmap) : this()
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

    public Song(Beatmap beatmap, AudioClip audioClip, Texture2D background) : this(beatmap, audioClip)
    {
        Background = background;
    }
}