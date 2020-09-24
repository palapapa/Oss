using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static bool IsOptionOpen = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
