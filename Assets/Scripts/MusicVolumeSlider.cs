using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour, IOnValueChanged
{
    private Slider slider;
    private AudioSource musicAudioSource;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        musicAudioSource = AudioChannels.Music.GetComponent<AudioSource>();
        musicAudioSource.volume = PlayerData.Instance.MusicVolume;
    }

    private void Update()
    {
        slider.value = PlayerData.Instance.MusicVolume;
        musicAudioSource.volume = PlayerData.Instance.MusicVolume * PlayerData.Instance.MasterVolume;
    }

    public void OnValueChanged()
    {
        PlayerData.Instance.MusicVolume = slider.value;
        PlayerData.SavePersistentPlayerData();
    }
}