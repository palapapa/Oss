using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour, IOnValueChanged
{
    private UnityEngine.UI.Slider slider;
    private AudioSource musicAudioSource;

    private void Start()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
        musicAudioSource = AudioChannels.Music.GetComponent<AudioSource>();
        musicAudioSource.volume = PlayerData.PersistentPlayerData.MusicVolume;
    }

    private void Update()
    {
        slider.value = PlayerData.PersistentPlayerData.MusicVolume;
        musicAudioSource.volume = PlayerData.PersistentPlayerData.MusicVolume * PlayerData.PersistentPlayerData.MasterVolume;
    }

    public void OnValueChanged()
    {
        PlayerData.PersistentPlayerData.MusicVolume = slider.value;
        PlayerData.SavePersistentPlayerData();
    }
}