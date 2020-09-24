using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject OptionsMenu;
    public static Options Instance;
    private Animator OptionsMenuAnimator;

    void Start()
    {
        OptionsMenuAnimator = OptionsMenu.GetComponent<Animator>();
    }
    public void ClickOptions()
    {
        if (!Data.IsOptionOpen)
        {
            OptionsMenuAnimator.SetTrigger("OptionsClicked");
            Data.IsOptionOpen = true;
        }
    }
    public void CloseOptions()
    {
        if (Data.IsOptionOpen)
        {
            OptionsMenuAnimator.SetTrigger("OptionsNotClicked");
            Data.IsOptionOpen = false;
        }
    }
    Options()
    {
        Instance = this;
    }
}
