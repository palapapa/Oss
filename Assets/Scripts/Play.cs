using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public GameObject SongSelection;
    public void ClickPlay()
    {
        SongSelection.SwitchPanelSingle();
    }
}
