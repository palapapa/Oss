using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audios : MonoBehaviour
{
    public static Audios Instance;//workaround to show audio clips in inspector
    public AudioClip Triangles;
    Audios()
    {
        Instance = this;
    }
}
