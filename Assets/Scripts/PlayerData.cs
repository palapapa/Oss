using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerData
{
    public static bool IsOptionActive { get; set; } = false;
    public static GameObject ActivePanel { get; set; }
    public static bool IsCreditsActive { get; set; } = false;
    public static bool HasIntroFinished { get; set; } = false;
    public static bool IsSongListActive { get; set; } = false;
    public static PersistentPlayerData PersistentPlayerData { get; set; } = new PersistentPlayerData(); // warper for all player data to be saved onto disk

    public static void SavePersistentPlayerData()
    {
        using (FileStream fileStream = File.Create(Application.persistentDataPath + "Oss.dat"))
        {
            try
            {
                new BinaryFormatter().Serialize(fileStream, PersistentPlayerData);
            }
            catch (SerializationException)
            {
                //todo: add pop up notification system
            }
        }
        Debug.Log("Player data saved");
    }

    public static void LoadPersistentPlayerData()
    {
        using (FileStream fileStream = File.OpenRead(Application.persistentDataPath + "Oss.dat"))
        {
            try
            {
                PersistentPlayerData = (PersistentPlayerData)new BinaryFormatter().Deserialize(fileStream);
            }
            catch (SerializationException)
            {
                PersistentPlayerData = new PersistentPlayerData();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Save file not found, falling back to default");
            }
        }
        Debug.Log("Player data loaded");
    }
}