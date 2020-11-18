using UnityEngine;

public class MusicPlayerList : MonoBehaviour, ILeftClickable
{
    public SongList SongList;
    public void OnLeftClick()
    {
        StartCoroutine(SongList.Activate());
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
