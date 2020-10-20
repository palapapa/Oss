[System.Serializable]
public class PersistentPlayerData
{
    public string BeatmapLocation { get; set; } = string.Empty;

    public PersistentPlayerData()
    {

    }
    public PersistentPlayerData(PlayerData playerData)
    {
        BeatmapLocation = playerData.BeatmapLocation;
    }
}