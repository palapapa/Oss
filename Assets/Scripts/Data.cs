using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static bool IsOptionOpen = false;
    public static GameObject ActivePanel;
    public static string BeatmapLocation;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
