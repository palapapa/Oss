using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit(0);//todo: add see you next time message
    }
}
