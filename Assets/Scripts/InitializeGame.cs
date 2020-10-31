using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    private void Awake()
    {
        PlayerData.LoadPersistentPlayerData();
    }
}