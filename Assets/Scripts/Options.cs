using UnityEngine;

public class Options : MonoBehaviour, ILeftClickable
{
    public GameObject OptionsMenu;
    public static Options Instance { get; set; }
    private Animator optionsMenuAnimator;

    private void Start()
    {
        optionsMenuAnimator = OptionsMenu.GetComponent<Animator>();
    }

    public void OnLeftClick()
    {
        Activate();
    }

    public void Activate()
    {
        if (!PlayerData.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsClicked");
            PlayerData.IsOptionActive = true;
        }
    }

    public void Deactivate()
    {
        if (PlayerData.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsNotClicked");
            PlayerData.IsOptionActive = false;
        }
    }

    private Options()
    {
        Instance = this;
    }
}