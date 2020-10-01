using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public void OnLeftClick()
    {
        SongSelection.SwitchPanelSingle();
    }
}
