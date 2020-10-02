using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audios : MonoBehaviour
{
    public static Audios Instance;//workaround to show audio clips in inspector
    public AudioClip Triangles;
    public AudioClip KeyDown0;
    public AudioClip KeyDown1;
    public AudioClip KeyDown2;
    [HideInInspector]
    public AudioClip[] KeyDownSounds;
    public AudioClip Backspace;
    private void Start()
    {
        DontDestroyOnLoad(this);
        KeyDownSounds = new AudioClip[] { KeyDown0, KeyDown1, KeyDown2 };
    }
    Audios()
    {
        Instance = this;
    }
}
