using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelection : MonoBehaviour
{
    public GameObject Content;
    public GameObject SongButton;
    public static SongSelection Instance;
    public void UpdateSongList()
    {
        for (int i = Content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < SongManager.Songs.Count; i++)
        {
            if (SongManager.Songs[i].MetadataSection.Title == "Triangles")
            {
                continue;
            }
            GameObject songListButton = Instantiate(SongButton);
            songListButton.transform.SetParent(Content.transform, false);
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)Content.transform);
            songListButton.transform.Find("Title").GetComponent<Text>().text = SongManager.Songs[i].MetadataSection.Title + "[" +
                SongManager.Songs[i].MetadataSection.Version + "]";
        }
    }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
}
