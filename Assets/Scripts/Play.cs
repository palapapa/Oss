using System.Collections;
using UnityEngine;

public class Play : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public GameObject MainMenu;
    public void OnLeftClick()
    {
        StartCoroutine(SwitchToSongSelection());
    }
    private IEnumerator SwitchToSongSelection()
    {
        yield return StartCoroutine(Utilities.FadeCanvasGroupTo(MainMenu, 0.15f, 20, 0.0f));
        SongSelection.transform.SetAsLastSibling();
        yield return StartCoroutine(Utilities.FadeCanvasGroup(SongSelection, 0.15f, 20, 0.0f, 1.0f));
    }
}