using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OsuLogo : MonoBehaviour
{
    public GameObject popUpButtonsLogo;
    public Button play;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ClickOsuLogo()
    {
        popUpButtonsLogo.SetActive(true);
        play.GetComponent<Animator>().SetTrigger("LogoPressed");
    }
}