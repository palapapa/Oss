using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour, ILeftClickable
{
    public static Credits Instance { get; set; }
    public GameObject CreditsPanel;

    public void OnLeftClick()
    {
        StartCoroutine(OpenCredits());
    }
    public IEnumerator OpenCredits()
    {
        if (!PlayerData.Instance.IsCreditsOpen)
        {
            CreditsPanel.SetActive(true);
            PlayerData.Instance.IsCreditsOpen = true;
            yield return StartCoroutine(CreditsPanel.FadeCanvasGroup(0.1f, 20, 0.0f, 1.0f));
        }
        else
        {
            yield break;
        }
    }
    public IEnumerator CloseCredits()
    {
        if (PlayerData.Instance.IsCreditsOpen)
        {
            PlayerData.Instance.IsCreditsOpen = false;
            yield return StartCoroutine(CreditsPanel.FadeCanvasGroup(0.1f, 20, 1.0f, 0.0f));
            CreditsPanel.SetActive(false);
        }
        else
        {
            yield break;
        }
    }
    private Credits()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
