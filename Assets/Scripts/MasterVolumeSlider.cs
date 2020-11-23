using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour, IOnValueChanged
{
    private Slider slider;
    
    public void OnValueChanged()
    {
        PlayerData.PersistentPlayerData.MasterVolume = slider.value;
        PlayerData.SavePersistentPlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerData.PersistentPlayerData.MasterVolume;
    }
}
