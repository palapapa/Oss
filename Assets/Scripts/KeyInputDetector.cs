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
                Audios.PlayAudio(Audios.Instance.Backspace, 1.0f, Audios.SoundEffects);
            }
            else
            {
                Audios.PlayAudio(Audios.Instance.KeyDownSounds[new System.Random().Next(0, Audios.Instance.KeyDownSounds.Length)], 1.0f, Audios.SoundEffects);//plays a random keydown sound
            }
        }
    }
}