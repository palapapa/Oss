using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelection : MonoBehaviour
{
    public GameObject Content;
    public GameObject Background;
    public GameObject SongButton;
    public Text SongName;
    public Text Mapper;
    public Text Length;
    public Text Bpm;
    public Text Objects;
    public Text Circles;
    public Text Sliders;
    public Text Spinners;
    public Text Cs;
    public Text Ar;
    public Text Od;
    public Text Hp;
    public Text Stars;
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
            GameObject songSelectionButton = Instantiate(SongButton);
            SongSelectionButton script = songSelectionButton.GetComponent<SongSelectionButton>();
            songSelectionButton.transform.SetParent(Content.transform, false);
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)Content.transform);
            script.Title.text = SongManager.Songs[i].MetadataSection.Title + "[" +
                SongManager.Songs[i].MetadataSection.Version + "]";
            script.Song = SongManager.Songs[i];
        }
    }

    void Start()
    {
        Instance = this;
    }
}
