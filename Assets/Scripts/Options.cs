using UnityEngine;

public class Options : MonoBehaviour, ILeftClickable
{
    public GameObject OptionsMenu;
    public static Options Instance;
    private Animator optionsMenuAnimator;

    private void Start()
    {
        optionsMenuAnimator = OptionsMenu.GetComponent<Animator>();
    }

    public void OnLeftClick()
    {
        OpenOptions();
    }
    public void OpenOptions()
    {
        if (!PlayerData.Instance.IsOptionOpen)
        {
            optionsMenuAnimator.SetTrigger("OptionsClicked");
            PlayerData.Instance.IsOptionOpen = true;
        }
    }
    public void CloseOptions()
    {
        if (PlayerData.Instance.IsOptionOpen)
        {
            optionsMenuAnimator.SetTrigger("OptionsNotClicked");
            PlayerData.Instance.IsOptionOpen = false;
        }
    }

    private Options()
    {
        Instance = this;
    }
}