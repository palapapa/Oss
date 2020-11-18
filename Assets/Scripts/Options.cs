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
        if (!PlayerData.Instance.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsClicked");
            PlayerData.Instance.IsOptionActive = true;
        }
    }

    public void Deactivate()
    {
        if (PlayerData.Instance.IsOptionActive)
        {
            optionsMenuAnimator.SetTrigger("OptionsNotClicked");
            PlayerData.Instance.IsOptionActive = false;
        }
    }

    private Options()
    {
        Instance = this;
    }
}