using UnityEngine;

public class KeyInputDetector : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SongSelection;
    private bool hasLoaded = false;

    private void Start()
    {
        if (!hasLoaded)
        {
            DontDestroyOnLoad(this);
            hasLoaded = true;
        }
        PlayerData.Instance.ActivePanel = MainMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerData.Instance.IsCreditsOpen)
            {
                StartCoroutine(Credits.Instance.CloseCredits());
            }
            else if (PlayerData.Instance.IsOptionOpen && PlayerData.Instance.ActivePanel == MainMenu)
            {
                Options.Instance.CloseOptions();
            }
            else if (PlayerData.Instance.ActivePanel == SongSelection)
            {
                StartCoroutine(Back.Instance.SwitchToMainMenu());
            }
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
                Audio.PlayAudio(Audio.Instance.Backspace, 1.0f, AudioChannels.SoundEffects);
            }
            else
            {
                Audio.PlayAudio(Audio.Instance.KeyDownSounds[new System.Random().Next(0, Audio.Instance.KeyDownSounds.Length)], 1.0f, AudioChannels.SoundEffects);//plays a random keydown sound
            }
        }
    }
}