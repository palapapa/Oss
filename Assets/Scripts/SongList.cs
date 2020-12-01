using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongList : MonoBehaviour
{
    public static SongList Instance { get; set; }
    public GameObject Content;
    public GameObject SongListButton;
    public IEnumerator Activate()
    {
        if (!PlayerData.IsSongListActive)
        {
            PlayerData.IsSongListActive = true;
            gameObject.SetActive(true);
            yield return StartCoroutine(gameObject.FadeCanvasGroup(0.2f, 100, 0.0f, 1.0f));
        }
    }
    public IEnumerator Deactivate()
    {
        if (PlayerData.IsSongListActive)
        {
            PlayerData.IsSongListActive = false;
            yield return StartCoroutine(gameObject.FadeCanvasGroup(0.2f, 100, 1.0f, 0.0f));
            gameObject.SetActive(false);
        }
    }
    public void UpdateSongList()
    {
        for (int i = Content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }
        List<Song> songs = SongManager.GetUniqueSongList();
        for (int i = 0; i < songs.Count; i++)
        {
            GameObject songListButton = Instantiate(SongListButton);
            songListButton.transform.SetParent(Content.transform, false);
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)Content.transform);
            SongListButton script = songListButton.GetComponent<SongListButton>();
            script.Title.text = songs[i].MetadataSection.Title;
            script.Song = songs[i];
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}
