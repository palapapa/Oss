using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(gameObject);
            hasLoaded = true;
        }
        Instance = this;
        saveFilePath = Application.persistentDataPath + @"\PersistentPlayerData.dat";
        if (File.Exists(saveFilePath))
        {
            persistentPlayerData = LoadPersistentPlayerData();
            BeatmapLocation = persistentPlayerData.BeatmapLocation;
        }
        //InvokeRepeating("SavePersistentPlayerData", 0.0f, 60.0f);
    }
    public static PersistentPlayerData SavePersistentPlayerData()
    {
        PersistentPlayerData persistentPlayerData = new PersistentPlayerData(Instance);
        using (FileStream fileStream = File.Create(saveFilePath))
        {
            new BinaryFormatter().Serialize(fileStream, persistentPlayerData);
        }
        return persistentPlayerData;
    }
    public static PersistentPlayerData LoadPersistentPlayerData()
    {
        using (FileStream fileStream = File.OpenRead(saveFilePath))
        {
            persistentPlayerData = (PersistentPlayerData)new BinaryFormatter().Deserialize(fileStream);
        }
        return persistentPlayerData;
    }
}
