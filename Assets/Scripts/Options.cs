using UnityEngine;

public class Options : MonoBehaviour, ILeftClickable
{
    public GameObject OptionsMenu;
    public static Options Instance { get; set; }
    private static Animator optionsMenuAnimator;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        optionsMenuAnimator = OptionsMenu.GetComponent<Animator>();
    }

    public void OnLeftClick()
    {
        Activate();
    }

    public static void Activate()
    {
        if (!PlayerData.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsClicked");
            PlayerData.IsOptionActive = true;
        }
    }

    public static void Deactivate()
    {
        if (PlayerData.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsNotClicked");
            PlayerData.IsOptionActive = false;
        }
    }
}