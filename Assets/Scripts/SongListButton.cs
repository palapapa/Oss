using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongListButton : MonoBehaviour, ILeftClickable
{
    public Text title;
    public void OnLeftClick()
    {
        MusicPlayer.PlaySelected(title.text, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
