using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectsVolumeSlider : MonoBehaviour, IOnValueChanged
{
    private Slider slider;
    private AudioSource soundEffectsAudioSource;

    public void OnValueChanged()
    {
        PlayerData.PersistentPlayerData.SoundEffectsVolume = slider.value;
        PlayerData.SavePersistentPlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        soundEffectsAudioSource = AudioChannels.SoundEffects.GetComponent<AudioSource>();
        soundEffectsAudioSource.volume = PlayerData.PersistentPlayerData.SoundEffectsVolume;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerData.PersistentPlayerData.SoundEffectsVolume;
        soundEffectsAudioSource.volume = PlayerData.PersistentPlayerData.SoundEffectsVolume * PlayerData.PersistentPlayerData.MasterVolume;
    }
}
