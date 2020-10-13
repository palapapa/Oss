using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public GameObject MainMenu;
    public void OnLeftClick()
    {
        StartCoroutine(SwitchToMainMenu());
    }
    private IEnumerator SwitchToMainMenu()
    {
        MainMenu.GetComponent<CanvasGroup>().alpha = 0;
        yield return StartCoroutine(SongSelection.FadeCanvasGroupTo(0.2f, 20, 0.0f));
        MainMenu.transform.SetAsLastSibling();
        yield return StartCoroutine(MainMenu.FadeCanvasGroupTo(0.2f, 20, 1.0f));
    }
}
