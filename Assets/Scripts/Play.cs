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
        SongSelection.GetComponent<CanvasGroup>().alpha = 0;
        yield return StartCoroutine(MainMenu.FadeCanvasGroupTo(0.2f, 20, 0.0f));
        SongSelection.transform.SetAsLastSibling();
        yield return StartCoroutine(SongSelection.FadeCanvasGroupTo(0.2f, 20, 1.0f));
    }
}