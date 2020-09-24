using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Data.IsOptionOpen)
        {
            Options.Instance.CloseOptions();
        }
    }
}
