[System.Serializable]
public class PersistentPlayerData
{
    public string BeatmapLocation { get; set; }
    public float MusicVolume { get; set; }
    public float SoundEffectsVolume { get; set; }

    public PersistentPlayerData()
    {
        BeatmapLocation = string.Empty;
        MusicVolume = 0.5f;
        SoundEffectsVolume = 1.0f;
    }
    public PersistentPlayerData(PlayerData playerData)
    {
        BeatmapLocation = playerData.BeatmapLocation;
        MusicVolume = playerData.MusicVolume;
    }
}