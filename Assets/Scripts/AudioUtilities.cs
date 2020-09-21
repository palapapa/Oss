using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtilities
{
    public static float PlayAudio(AudioClip ac)
    {
        GameObject gameObject = new GameObject(ac.name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(ac);
        return Time.time;
    }
}
