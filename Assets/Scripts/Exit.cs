using UnityEngine;

public class Exit : MonoBehaviour, ILeftClickable
{
    public void OnLeftClick()
    {
        Application.Quit(0);//todo: add see you next time message
    }
}