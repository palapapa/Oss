using System.Collections;
using UnityEngine;

public class Back : MonoBehaviour, ILeftClickable
{
    public GameObject SongSelection;
    public GameObject MainMenu;
    public static Back Instance;

    public void OnLeftClick()
    {
        StartCoroutine(SwitchToMainMenu());
    }

    public IEnumerator SwitchToMainMenu()
    {
        MainMenu.GetComponent<CanvasGroup>().alpha = 0;
        yield return StartCoroutine(SongSelection.FadeCanvasGroupTo(0.2f, 1000, 0.0f));
        MainMenu.SwitchPanel();
        yield return StartCoroutine(MainMenu.FadeCanvasGroupTo(0.2f, 1000, 1.0f));
    }

    private Back()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}