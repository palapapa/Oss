using System.Collections;
using UnityEngine;

public class Play : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public GameObject MainMenu;
    public static Play Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnLeftClick()
    {
        StartCoroutine(SwitchToSongSelection());
    }

    public IEnumerator SwitchToSongSelection()
    {
        SongSelection.GetComponent<CanvasGroup>().alpha = 0;
        yield return StartCoroutine(MainMenu.FadeCanvasGroupTo(0.2f, 1000, 0.0f));
        SongSelection.SwitchPanel();
        yield return StartCoroutine(SongSelection.FadeCanvasGroupTo(0.2f, 1000, 1.0f));
    }
}