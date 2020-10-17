using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool IsOptionOpen { get; set; } = false;
    public string BeatmapLocation { get; set; }
    public GameObject ActivePanel { get; set; }
    public static PlayerData Instance { get; set; }
    private bool hasLoaded = false;
    private static string saveFilePath;
    private static PersistentPlayerData persistentPlayerData;

    private PlayerData()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(gameObject);
            hasLoaded = true;
        }
        saveFilePath = Application.persistentDataPath + @"\PersistentPlayerData.dat";
        if (File.Exists(saveFilePath))
        {
            persistentPlayerData = LoadPersistentPlayerData();
        }
        //InvokeRepeating("SavePersistentPlayerData", 0.0f, 60.0f);
    }

    public static PersistentPlayerData SavePersistentPlayerData()
    {
        PersistentPlayerData persistentPlayerData = new PersistentPlayerData(Instance);
        using (FileStream fileStream = File.Create(saveFilePath))
        {
            try
            {
                new BinaryFormatter().Serialize(fileStream, persistentPlayerData);
            }
            catch (SerializationException)
            {
                //todo: add pop up notification system
            }
        }
        return persistentPlayerData;
    }

    public static PersistentPlayerData LoadPersistentPlayerData()
    {
        using (FileStream fileStream = File.OpenRead(saveFilePath))
        {
            try
            {
                persistentPlayerData = (PersistentPlayerData)new BinaryFormatter().Deserialize(fileStream);
            }
            catch (SerializationException)
            {
                persistentPlayerData = new PersistentPlayerData(Instance);
            }
        }
        Instance.BeatmapLocation = persistentPlayerData.BeatmapLocation;
        Debug.Log($"Song location = {Instance.BeatmapLocation}");
        return persistentPlayerData;
    }
}