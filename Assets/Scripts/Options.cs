using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour, ILeftClickable
{
    public GameObject OptionsMenu;
    public static Options Instance;
    private Animator optionsMenuAnimator;

    void Start()
    {
        optionsMenuAnimator = OptionsMenu.GetComponent<Animator>();
    }
    public void OnLeftClick()
    {
        if (!Data.IsOptionOpen)
        {
            optionsMenuAnimator.SetTrigger("OptionsClicked");
            Data.IsOptionOpen = true;
        }
    }
    public void CloseOptions()
    {
        if (Data.IsOptionOpen)
        {
            optionsMenuAnimator.SetTrigger("OptionsNotClicked");
            Data.IsOptionOpen = false;
        }
    }
    Options()
    {
        Instance = this;
    }
}
