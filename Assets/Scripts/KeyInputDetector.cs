using UnityEngine;

public class KeyInputDetector : MonoBehaviour
{
    private static bool hasLoaded = false;

    private void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerData.Instance.IsOptionOpen)
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
                Audio.PlayAudio(Audio.Instance.Backspace, 1.0f, Audio.SoundEffects);
            }
            else
            {
                Audio.PlayAudio(Audio.Instance.KeyDownSounds[new System.Random().Next(0, Audio.Instance.KeyDownSounds.Length)], 1.0f, Audio.SoundEffects);//plays a random keydown sound
            }
        }
    }
}