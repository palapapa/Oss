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
        if
        (
            !Input.GetKeyDown(KeyCode.Mouse0) &&
            !Input.GetKeyDown(KeyCode.Mouse1) &&
            !Input.GetKeyDown(KeyCode.Mouse2) &&
            !Input.GetKeyDown(KeyCode.Mouse3) &&
            !Input.GetKeyDown(KeyCode.Mouse4) &&
            !Input.GetKeyDown(KeyCode.Mouse5) &&
            !Input.GetKeyDown(KeyCode.Mouse6) &&
            Input.anyKeyDown
        )
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                AudioSource.PlayClipAtPoint(Audios.Instance.Backspace, Camera.main.transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(Audios.Instance.KeyDownSounds[new System.Random().Next(0, Audios.Instance.KeyDownSounds.Length)], Camera.main.transform.position);//plays a random keydown sound
            }
        }
    }
}
