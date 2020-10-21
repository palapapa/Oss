using UnityEngine;

public class AudioChannels : MonoBehaviour
{
    public static GameObject Music { get; set; }
    public static GameObject SoundEffects { get; set; }
    private bool hasLoaded = false;

    private void Awake()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
        Music = new GameObject
        {
            name = "Music"
        };
        SoundEffects = new GameObject
        {
            name = "SoundEffects"
        };
        Music.AddComponent<AudioSource>();
        SoundEffects.AddComponent<AudioSource>();
        Music.transform.SetParent(transform);
        SoundEffects.transform.SetParent(transform);
    }

    private void Start()
    {
        
    }
}