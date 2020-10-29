using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, IOnValueChanged
{
    private Slider slider;
    private AudioSource audioSource;
    
    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        audioSource = AudioChannels.Music.GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        audioSource.volume = slider.value;
    }

    public void OnValueChanged()
    {
        
    }
}
