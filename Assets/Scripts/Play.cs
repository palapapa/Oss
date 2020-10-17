using System.Collections;
using UnityEngine;

public class Play : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public GameObject MainMenu;
    public static Play Instance;
    public void OnLeftClick()
    {
        StartCoroutine(SongManager.ScanSongDirectory());
        StartCoroutine(SwitchToSongSelection());
    }
    public IEnumerator SwitchToSongSelection()
    {
        SongSelection.GetComponent<CanvasGroup>().alpha = 0;
        yield return StartCoroutine(MainMenu.FadeCanvasGroupTo(0.2f, 20, 0.0f));
        SongSelection.SwitchPanel();
        yield return StartCoroutine(SongSelection.FadeCanvasGroupTo(0.2f, 20, 1.0f));
    }
    private Play()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}