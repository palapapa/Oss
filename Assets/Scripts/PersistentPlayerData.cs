[System.Serializable]
public class PersistentPlayerData
{
    public string BeatmapLocation { get; set; }

    public PersistentPlayerData(PlayerData playerData)
    {
        BeatmapLocation = playerData.BeatmapLocation;
    }
}