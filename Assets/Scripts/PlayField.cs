using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public static PlayField Instance { get; set; }
    public GameObject SongSelection;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator SwitchToPlayField()
    {
        yield return StartCoroutine(SongSelection.FadeCanvasGroup(0.5f, 100, 1.0f, 0.0f));
        gameObject.SwitchPanel();
        yield return StartCoroutine(gameObject.FadeCanvasGroup(0.5f, 100, 0.0f, 1.0f));
    }
}
