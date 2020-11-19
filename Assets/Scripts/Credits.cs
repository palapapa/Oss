using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour, ILeftClickable
{
    public static Credits Instance { get; set; }
    public GameObject CreditsPanel;

    public void OnLeftClick()
    {
        StartCoroutine(Activate());
    }

    public IEnumerator Activate()
    {
        if (!PlayerData.Instance.IsCreditsActive)
        {
            CreditsPanel.SetActive(true);
            PlayerData.Instance.IsCreditsActive = true;
            yield return StartCoroutine(CreditsPanel.FadeCanvasGroup(0.1f, 10000, 0.0f, 1.0f)); // todo: use animator instead of this inconsistent shit
        }
        else
        {
            yield break;
        }
    }

    public IEnumerator Deactivate()
    {
        if (PlayerData.Instance.IsCreditsActive)
        {
            PlayerData.Instance.IsCreditsActive = false;
            yield return StartCoroutine(CreditsPanel.FadeCanvasGroup(0.1f, 10000, 1.0f, 0.0f));
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